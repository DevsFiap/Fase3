using Fase03.Domain.Models;
using Fase03.Infra.Message.Settings;
using Fase03.Infra.Message.ValueObjects;
using Fase03.Infra.Messages.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Fase03.Application.Interfaces;
using Fase03.Application.Commands;

namespace Fase03.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly MessageSettings _messageSettings;
        private readonly ILogger<Worker> _logger;
        private readonly IMailHelper _mailHelper;
        private readonly IServiceProvider _serviceProvider;
        //  private readonly IContatosAppService _contatosAppService;

        public Worker(
            IConnection connection,
            IModel model,
            IOptions<MessageSettings> messageSettings,
            IMailHelper mailHelper,
            ILogger<Worker> logger,
            IServiceProvider serviceProvider
            //      IContatosAppService contatosAppService
            )
        {
            _connection = connection;
            _model = model;
            _messageSettings = messageSettings.Value;
            _mailHelper = mailHelper;
            _logger = logger;
            _serviceProvider = serviceProvider;
   //         _contatosAppService = contatosAppService;

            _logger.LogInformation("Iniciando o Worker...");

            if (_connection.IsOpen)
                _logger.LogInformation("Conexão com RabbitMQ estabelecida com sucesso.");
            else
                _logger.LogError("Erro ao conectar ao RabbitMQ.");

            if (_model.IsOpen)
                _logger.LogInformation("Canal (IModel) aberto com sucesso.");
            else
                _logger.LogError("Erro ao abrir o canal RabbitMQ.");
        }

        /// <summary>
        /// Método para ler a fila do RabbitMQ
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando o Worker...");

            try
            {
                // Declarando a fila e o exchange
                _model.QueueDeclare(
                    queue: _messageSettings.Queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: new Dictionary<string, object>
                    {
                        { "x-dead-letter-exchange", "dlx_exchange" },
                        { "x-dead-letter-routing-key", "dlx_routing_key" }
                    }
                );

                _model.ExchangeDeclare("dlx_exchange", ExchangeType.Direct, durable: true);
                _model.QueueBind("dlq_queue", "dlx_exchange", "dlx_routing_key");

                _logger.LogInformation("Fila 'contato' e DLX configurados com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Erro ao configurar o RabbitMQ: {ex.Message}");
            }

            // Configuração do consumer
            var consumer = new EventingBasicConsumer(_model);

            consumer.Received += async (sender, args) =>
            {
                var contentArray = args.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);

                _logger.LogInformation($"Mensagem recebida: {contentString}");

                try
                {
                    var messageQueueModel = JsonConvert.DeserializeObject<MessageQueueModel>(contentString);
                    _logger.LogInformation($"Tipo de mensagem: {messageQueueModel.Tipo}");

                    if (messageQueueModel.Tipo == TipoMensagem.INSERIR_CONTATO)
                    {
                        var scope = _serviceProvider.CreateScope();
                        var contatosAppService = scope.ServiceProvider.GetRequiredService<IContatosAppService>();
                        var criarContatoCommand = JsonConvert.DeserializeObject<CriarContatoCommand>(messageQueueModel.Conteudo);
                        var contatoDto = await contatosAppService.CriarContatoAsync(criarContatoCommand);
                        _logger.LogInformation($"Contato '{contatoDto.Nome}' criado com sucesso.");

                        var contatosMessageVO = new ContatosMessageVO
                        {
                            Nome = contatoDto.Nome,
                            Telefone = contatoDto.NumeroTelefone,
                            Email = contatoDto.Email
                        };

                        // Envia o e-mail e registra log de sucesso/erro
                        //EnviarMensagemDeConfirmacaoDeCadastro(contatosMessageVO);
                    }
                    else
                    {
                        _logger.LogWarning("Tipo de mensagem não implementado para processamento.");
                    }

                    // Para testar o DLQ, rejeitamos a mensagem (sem requeue)
                    //_model.BasicReject(args.DeliveryTag, requeue: false);
                    //_logger.LogInformation("Mensagem rejeitada e enviada para a DLQ.");

                    //Para consumir a mensagem da fila e salvar no banco de dados
                    _model.BasicAck(args.DeliveryTag, false);
                    _logger.LogInformation("Mensagem confirmada (BasicAck) e consumida.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao processar a mensagem: {ex.Message}");
                    _model.BasicNack(args.DeliveryTag, false, false);
                    _logger.LogError("Mensagem rejeitada (BasicNack).");
                }
            };

            _logger.LogInformation("Consumidor configurado e aguardando mensagens...");
            _model.BasicConsume(queue: _messageSettings.Queue, autoAck: false, consumerTag: "", consumer: consumer);
            _logger.LogInformation("Iniciando o consumo da fila: " + _messageSettings.Queue);

            await Task.Delay(Timeout.Infinite, stoppingToken);
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
                    <br/><br/>
                    <strong>Parabéns, seu contato foi criado com sucesso!</strong>
                    <br/><br/>
                    ID: <strong>{contatosMessageVO.Id}</strong> <br/>
                    Nome: <strong>{contatosMessageVO.Nome}</strong> <br/>
                    Telefone: <strong>{contatosMessageVO.Telefone}</strong> <br/><br/>
                    Att, <br/>
                    Equipe FIAP.";

            try
            {
                _mailHelper.Send(mailTo, subject, body);
                _logger.LogInformation($"E-mail de confirmação enviado para {contatosMessageVO.Email} com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar e-mail de confirmação para {contatosMessageVO.Email}: {ex.Message}");
            }
        }
    }
}