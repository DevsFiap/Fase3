using Fase03.Application.Commands;
using Fase03.Application.Interfaces;
using Fase03.Application.Services;
using Fase03.Domain.Models;
using Fase03.Infra.Message.Settings;
using Fase03.Infra.Message.ValueObjects;
using Fase03.Infra.Messages.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Fase03.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly MessageSettings? _messageSettings;
      //  private readonly IContatosAppService _contatosAppService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMailHelper _mailHelper;
        private readonly IConnection? _connection;
        private readonly IModel? _model;

        public Worker(IOptions<MessageSettings> messageSettings,
    //        IContatosAppService contatosAppService,
            IServiceProvider serviceProvider,
            IMailHelper mailHelper)
        {
            _messageSettings = messageSettings.Value;
      //      _contatosAppService = contatosAppService;
            _serviceProvider = serviceProvider;
            _mailHelper = mailHelper;

            #region Conectando no servidor de mensageria
            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    Uri = new Uri(_messageSettings.Host)
                };

                _connection = connectionFactory.CreateConnection();
                _model = _connection.CreateModel();

                Console.WriteLine("✅ Conexão com RabbitMQ estabelecida com sucesso!");

                _model.QueueDeclare(
                    queue: _messageSettings.Queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: new Dictionary<string, object>
                    {
                        { "x-dead-letter-exchange", "dlx_exchange" } // DLX configurado para redirecionar para o DLQ
                    }
                );

                _model.ExchangeDeclare("dlx_exchange", ExchangeType.Direct);

                _model.QueueBind("dlq_queue", "dlx_exchange", "dlx_routing_key");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
            #endregion

        }

        /// <summary>
        /// Método para ler a fila do RabbitMQ
        /// </summary>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //componente para fazer a leitura da fila
            var consumer = new EventingBasicConsumer(_model);

            //fazendo a leitura
            consumer.Received += async (sender, args) =>
            {
                //ler o conteudo da mensagem gravada na fila
                var contentArray = args.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);

                //deserializar a mensagem
                var messageQueueModel = JsonConvert.DeserializeObject<MessageQueueModel>(contentString);

                //verificar o tipo da mensagem
                switch (messageQueueModel.Tipo)
                {
                    case TipoMensagem.INSERIR_CONTATO:
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            try
                            {
                                var contatosAppService = scope.ServiceProvider.GetRequiredService<IContatosAppService>();
                                var criarContato = JsonConvert.DeserializeObject<CriarContatoCommand>
                                (messageQueueModel.Conteudo);
                                var contato = await contatosAppService.CriarContatoAsync(criarContato);
                                _model.BasicAck(args.DeliveryTag, false);
                            }
                            catch(Exception e)
                            {
                                _model.BasicNack(args.DeliveryTag, false, false);
                            }
                        }
                        break;

                    case TipoMensagem.ATUALIZAR_CONTATO:
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            try
                            {
                                var contatosAppService = scope.ServiceProvider.GetRequiredService<IContatosAppService>();
                                var atualizarContato = JsonConvert.DeserializeObject<ContatosMessageVO>
                                (messageQueueModel.Conteudo);

                                var atualizarContatoCmd = new AtualizarContatoCommand
                                {
                                    Nome = atualizarContato.Nome,
                                    Telefone = atualizarContato.Telefone,
                                    Email = atualizarContato.Email
                                };

                                var contato = await contatosAppService.AtualizarContatoAsync((int)atualizarContato.Id, atualizarContatoCmd);
                                _model.BasicAck(args.DeliveryTag, false);
                            }
                            catch (Exception e)
                            {
                                _model.BasicNack(args.DeliveryTag, false, false);
                            }
                        }
                        break;
                    case TipoMensagem.DELETAR_CONTATO:
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            try
                            {
                                var contatosAppService = scope.ServiceProvider.GetRequiredService<IContatosAppService>();
                                var deletarContato = JsonConvert.DeserializeObject<ContatosMessageVO>
                                (messageQueueModel.Conteudo);

                                await contatosAppService.ExcluirContatoAsync((int)deletarContato.Id);
                                _model.BasicAck(args.DeliveryTag, false);
                            }
                            catch (Exception e)
                            {
                                _model.BasicNack(args.DeliveryTag, false, false);
                            }
                        }
                        break;
                }
            };
            //executando o consumidor
            _model.BasicConsume(queue: _messageSettings.Queue, autoAck: false, consumerTag: "", consumer: consumer);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Método para escrever e enviar o email
        /// de confirmação de cadastro de conta de contato
        /// </summary>
        private void EnviarMensagemDeConfirmacaoDeCadastro(ContatosMessageVO contatosMessageVO)
        {
            var mailTo = contatosMessageVO.Email;
            var subject = $"Confirmação de cadastro de contato. ID: {contatosMessageVO.Id}";
            var body = $@"Olá {contatosMessageVO.Nome},
                <br/>
                <br/>
                <strong>Parabéns, seu contato
                foi criado com sucesso!</strong>
                <br/>
                <br/>
                ID: <strong>{contatosMessageVO.Id}</strong> <br/>
                Nome: <strong>{contatosMessageVO.Nome}</strong> <br/>
                Telefone: <strong>{contatosMessageVO.Telefone}</strong> <br/>
                <br/>
                Att, <br/>
                Equipe FIAP.";

            _mailHelper.Send(mailTo, subject, body);
        }
    }
}
