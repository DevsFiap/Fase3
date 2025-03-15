using Fase03.Domain.Interfaces.Messages;
using Fase03.Domain.Models;
using Fase03.Infra.Message.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Fase03.Infra.Message.Producers;

/// <summary>
/// Classe para escrita de mensagens na fila do RabbitMQ
/// </summary>
public class MessageQueueProducer : IMessageQueueProducer
{
    private readonly MessageSettings? _messageSettings;
    private readonly ConnectionFactory? _connectionFactory;

    public MessageQueueProducer(IOptions<MessageSettings> messageSettings)
    {
        this._messageSettings = messageSettings.Value;

        //Conexão com o servidor de mensageria(broker)
        _connectionFactory = new ConnectionFactory
        {
            //HostName = _messageSettings.Host,
            //UserName = _messageSettings.Username,
            //Password = _messageSettings.Password
            Uri = new Uri(_messageSettings.Host)
        };
    }

    /// <summary>
    /// Método para escrever uma mensagem na fila
    /// </summary>
    public void Create(MessageQueueModel model)
    {
        //Abrir conexão com o servidor de mensageria
        using (var connection = _connectionFactory.CreateConnection())
        {
            //Criando um objeto na fila de mensagens
            using (var channel = connection.CreateModel())
            {
                //Parametros para conexão da fila
                channel.QueueDeclare(
                    queue: _messageSettings.Queue, //Nome da fila
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: new Dictionary<string, object>
                    {
                        { "x-dead-letter-exchange", "dlx_exchange" } // DLX configurado para redirecionar para o DLQ
                    }
                );

                channel.ExchangeDeclare("dlx_exchange", ExchangeType.Direct);
                channel.QueueDeclare(queue: "dlq_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false);

                channel.QueueBind("dlq_queue", "dlx_exchange", "dlx_routing_key");

                //Escrevendo o conteúdo da fila
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: _messageSettings.Queue,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model))
                    );
            }
        }
    }
}