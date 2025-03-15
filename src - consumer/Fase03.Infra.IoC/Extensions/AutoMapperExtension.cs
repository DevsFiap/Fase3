using Microsoft.Extensions.DependencyInjection;
using Fase03.Application.Mappings;

namespace Fase03.Infra.IoC.Extensions;

public static class AutoMapperExtension
{
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DtoToEntityMap));
        return services;
    }
}