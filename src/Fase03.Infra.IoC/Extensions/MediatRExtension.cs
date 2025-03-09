using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Fase03.Infra.IoC.Extensions;

public static class MediatRExtension
{
    public static IServiceCollection AddMediatRConfig(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        return services;
    }
}