using Fase03.Infra.Message.Helpers;
using Fase03.Infra.Message.Settings;
using Fase03.Infra.Messages.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fase03.Infra.IoC.Extensions
{
    public static class MailHelperExtension
    {
        public static IServiceCollection AddMailHelperConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            //Registra o serviço de envio de e-mail (MailHelper)
            services.AddSingleton<IMailHelper, MailHelper>();

            return services;
        }
    }
}
