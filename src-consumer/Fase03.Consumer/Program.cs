using Fase03.Consumer;
using Fase03.Infra.IoC.Extensions;
using Fase03.Infra.Message.Settings;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddMediatRConfig();
builder.Services.AddDependencyInjection();
builder.Services.AddAutoMapperConfig();
builder.Services.AddDbContextConfig(builder.Configuration);
builder.Services.AddMailHelperConfig(builder.Configuration);
builder.Services.AddRabbitMqConfig(builder.Configuration);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var host = builder.Build();

// Recupera o logger da DI
var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Aplicação iniciando...");

host.Run();