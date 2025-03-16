using Fase03.Infra.Message.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Fase03.Infra.IoC.Extensions;

public static class RabbitMqExtension
{
    public static IServiceCollection AddRabbitMqConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração do RabbitMQ a partir do appsettings.json
        services.Configure<MessageSettings>(configuration.GetSection("MessageSettings"));

        services.AddSingleton<IConnection>(sp =>
        {
            var messageSettings = sp.GetRequiredService<IOptions<MessageSettings>>().Value;
            if (string.IsNullOrEmpty(messageSettings.Host))
            {
                throw new InvalidOperationException("MessageSettings.Host não está configurado. Verifique seu appsettings.json.");
            }
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(messageSettings.Host)
            };

            return connectionFactory.CreateConnection();
        });


        // Configura o canal (IModel) para o RabbitMQ
        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            return connection.CreateModel();
        });

        return services;
    }
}