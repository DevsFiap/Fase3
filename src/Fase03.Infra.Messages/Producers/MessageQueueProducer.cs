using Fase03.Domain.Interfaces.Messages;
using Fase03.Domain.Models;
using Fase03.Infra.Message.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace Fase03.Infra.Message.Producers;

/// <summary>
/// Classe para escrita de mensagens na fila do RabbitMQ
/// </summary>
public class MessageQueueProducer : IMessageQueueProducer
{
    private readonly MessageSettings? _messageSettings;
    private readonly ConnectionFactory? _connectionFactory;
    private readonly ILogger<MessageQueueProducer> _logger;

    public MessageQueueProducer(IOptions<MessageSettings> messageSettings, ILogger<MessageQueueProducer> logger)
    {
        _logger = logger;
        this._messageSettings = messageSettings.Value;

        //Conexão com o servidor de mensageria(broker)
        _connectionFactory = new ConnectionFactory { Uri = new Uri(_messageSettings.Host) };
    }

    /// <summary>
    /// Método para escrever uma mensagem na fila
    /// </summary>
    public void Create(MessageQueueModel model)
    {
        // Abre conexão com o servidor de mensageria
        using (var connection = _connectionFactory.CreateConnection())
        {
            _logger.LogInformation("Connectando no rabbit");
            // Cria o canal para envio de mensagens
            using (var channel = connection.CreateModel())
            {

                _logger.LogInformation("Criano Channel rabbit");
                // Declara a fila principal (idempotente)
                channel.QueueDeclare(
                    queue: _messageSettings.Queue, // Nome da fila ("contato")
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: new Dictionary<string, object>
                    {
                    { "x-dead-letter-exchange", "dlx_exchange" },
                    { "x-dead-letter-routing-key", "dlx_routing_key" }  // Define a routing key para a DLQ
                    }
                );

                _logger.LogInformation("Criando fila");
                // Declara o exchange para DLQ (idempotente)
                channel.ExchangeDeclare("dlx_exchange", ExchangeType.Direct, durable: true);

                _logger.LogInformation("Criando dlq");
                // Declara a fila para DLQ (idempotente)
                channel.QueueDeclare(
                    queue: "dlq_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                _logger.LogInformation("Criando fila dlq");
                // Faz a binding da DLQ com o exchange usando a routing key
                channel.QueueBind("dlq_queue", "dlx_exchange", "dlx_routing_key");
                _logger.LogInformation("Criando bind dlq");
                // Publica a mensagem na fila principal
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: _messageSettings.Queue,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model))
                );
                _logger.LogInformation("Publicando mensagem");
            }
        }
    }
}