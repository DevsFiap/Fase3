using Fase03.Application.Interfaces;
using Fase03.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fase03.Infra.IoC.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        //Application Layer
        services.AddScoped<IContatosAppService, ContatosAppService>();

        return services;
    }
}