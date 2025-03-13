using Fase03.Domain.Models;
using Fase03.Infra.Message.Settings;
using Fase03.Infra.Message.ValueObjects;
using Fase03.Infra.Messages.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Fase03.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly MessageSettings? _messageSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMailHelper _mailHelper;
        private readonly IConnection? _connection;
        private readonly IModel? _model;

        public Worker(IOptions<MessageSettings>
            messageSettings, IServiceProvider
            serviceProvider, IMailHelper mailHelper)
        {
            _messageSettings = messageSettings.Value;
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
                    arguments: null
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao conectar ao RabbitMQ: {ex.Message}");
            }

            /* 
                        var connectionFactory = new ConnectionFactory
                        {
                            //HostName = _messageSettings.Host,
                            //UserName = _messageSettings.Username,
                            //Password = _messageSettings.Password,
                            Uri = new Uri(_messageSettings.Host)
                        };

                        _connection = connectionFactory.CreateConnection();
                        _model = _connection.CreateModel();
                        _model.QueueDeclare(
                        queue: _messageSettings.Queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
            */
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
            consumer.Received += (sender, args) =>
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
                        //processando a mensagem
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            //capturando os dados do contato
                            //contido na mensagem
                            var contatosMessageVO = JsonConvert.DeserializeObject
                            <ContatosMessageVO>
                            (messageQueueModel.Conteudo);

                            //enviando o email
                     //       EnviarMensagemDeConfirmacaoDeCadastro(contatosMessageVO);

                            //comunicando ao rabbit que a mensagem
                            //foi processada!
                            //dessa forma, a mensagem sairá da fila
                            _model.BasicAck(args.DeliveryTag, false);
                        }
                        break;

                    case TipoMensagem.ATUALIZAR_CONTATO:
                        //TODO
                        break;
                    case TipoMensagem.DELETAR_CONTATO:
                        //TODO
                        break;
                }
            };
            //executando o consumidor
            _model.BasicConsume(queue: _messageSettings.Queue, autoAck: false, consumerTag: "", consumer: consumer);


            /*   while (!stoppingToken.IsCancellationRequested)
               {
                   Thread.Sleep(1000); // Mantém o loop ativo
               } */
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
