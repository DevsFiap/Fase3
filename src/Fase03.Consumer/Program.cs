using Fase03.Consumer;
using Fase03.Infra.IoC.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMailHelperConfig(builder.Configuration);
builder.Services.AddRabbitMqConfig(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
