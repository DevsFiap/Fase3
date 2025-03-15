using Microsoft.Extensions.DependencyInjection;
using Fase03.Application.Interfaces;
using Fase03.Application.Services;
using Fase03.Domain.Interfaces.Repositories;
using Fase03.Domain.Interfaces.Services;
using Fase03.Domain.Services;
using Fase03.Infra.Data.Repository;

namespace Fase03.Infra.IoC.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        //Application Layer
        services.AddScoped<IContatosAppService, ContatosAppService>();

        //Domain Layer
        services.AddScoped<IContatoDomainService, ContatoDomainService>();

        //Infrastructure Layer
        services.AddScoped<IContatosRepository, ContatoRepository>();

        return services;
    }
}