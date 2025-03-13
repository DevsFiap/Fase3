using Microsoft.Extensions.DependencyInjection;
using Fase03.Infra.Message.Consumers;
using Fase03.Infra.Message.Producers;
using Fase03.Infra.Message.Settings;
using Microsoft.Extensions.Configuration;
using Fase03.Infra.Message.Helpers;
using Fase03.Infra.Messages.Helpers;
using Fase03.Domain.Interfaces.Messages;

namespace Fase03.Infra.IoC.Extensions;

public static class RabbitMqExtension
{
    public static IServiceCollection AddRabbitMqConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração do RabbitMQ a partir do appsettings.json
        services.Configure<MessageSettings>(configuration.GetSection("MessageSettings"));

        // Registra o Producer (quem envia mensagens)
        services.AddSingleton<IMessageQueueProducer, MessageQueueProducer>();

        // Registra o Consumer (quem recebe mensagens)
      //  services.AddHostedService<MessageQueueConsumer>();

        return services;
    }
}