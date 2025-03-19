using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Fase03.Api.Extensions;

public static class SwaggerDocExtension
{
    public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Version = "v1",
                Title = "API - Contatos",
                Description = "API REST para cadastro de contatos",
                Contact = new OpenApiContact
                {
                    Name = "FIAP",
                    Email = "fiap@fiap.com",
                    Url = new Uri("https://postech.fiap.com.br/")
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Cabeçalho de autorização JWT usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();

        app.UseDeveloperExceptionPage();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API - Contatos");
        });

        return app;
    }
}