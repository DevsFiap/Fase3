﻿using Fase03.Domain.Interfaces.Messages;
using Fase03.Infra.Message.Producers;
using Fase03.Infra.Message.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fase03.Infra.IoC.Extensions;

public static class RabbitMqExtension
{
    public static IServiceCollection AddRabbitMqConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração do RabbitMQ a partir do appsettings.json
        services.Configure<MessageSettings>(configuration.GetSection("MessageSettings"));

        // Registra o Producer (quem envia mensagens)
        services.AddSingleton<IMessageQueueProducer, MessageQueueProducer>();

        return services;
    }
}