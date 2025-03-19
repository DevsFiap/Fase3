using Fase03.Api.Extensions;
using Fase03.Api.Middlewares;
using Fase03.Infra.IoC.Extensions;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddSwaggerDoc();
builder.Services.AddCorsPolicy();
builder.Services.AddDependencyInjection();
builder.Services.AddHttpContextAccessor();
builder.Services.AddPrometheusMetrics();
builder.Services.AddRabbitMqConfig(builder.Configuration);
builder.Services.AddMediatRConfig();

var app = builder.Build();

app.UseSwaggerDoc(app.Environment);
app.UseRouting();
app.UseMetricServer(); // Expõe /metrics
app.UseHttpMetrics(); // Métricas padrão do Prometheus
app.UsePrometheusCustomMetrics(); // Middleware para métricas customizadas
app.UseMiddleware<ExceptionMiddleware>();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseCorsPolicy();
app.MapControllers();
app.Run();

public partial class Program { }