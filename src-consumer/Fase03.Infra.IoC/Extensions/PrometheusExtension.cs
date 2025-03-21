using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace Fase03.Infra.IoC.Extensions
{
    public static class PrometheusMetricsExtension
    {
        private static readonly Counter RequestCounter = Metrics.CreateCounter(
            "http_requests_total",
            "Total de requisições recebidas, categorizadas por método HTTP e status.",
            new CounterConfiguration
            {
                LabelNames = new[] { "method", "status_code" }
            });

        private static readonly Histogram RequestDuration = Metrics.CreateHistogram(
            "http_request_duration_seconds",
            "Duração das requisições HTTP, categorizada por endpoint.",
            new HistogramConfiguration
            {
                LabelNames = new[] { "endpoint" },
                Buckets = Histogram.ExponentialBuckets(start: 0.01, factor: 2, count: 10)
            });

        private static readonly Gauge CpuUsageGauge = Metrics.CreateGauge(
            "process_cpu_usage",
            "Uso de CPU do processo como percentual.");

        private static readonly Gauge MemoryUsageGauge = Metrics.CreateGauge(
            "process_memory_usage_bytes",
            "Uso de memória do processo em bytes.");

        public static IServiceCollection AddPrometheusMetrics(this IServiceCollection services)
        {
            // Middleware de monitoramento
            services.AddHttpContextAccessor();
            return services;
        }

        public static IApplicationBuilder UsePrometheusCustomMetrics(this IApplicationBuilder app)
        {
            // Middleware para medir a latência por endpoint e contagem por status de resposta
            app.Use(async (context, next) =>
            {
                var endpoint = context.Request.Path;
                var method = context.Request.Method;

                // Cronômetro para medir a duração
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                await next();

                stopwatch.Stop();

                var statusCode = context.Response.StatusCode.ToString();

                // Incrementa contadores e histogramas
                RequestCounter.WithLabels(method, statusCode).Inc();
                RequestDuration.WithLabels(endpoint).Observe(stopwatch.Elapsed.TotalSeconds);
            });

            // Monitoramento de recursos do sistema
            app.Use(async (context, next) =>
            {
                CpuUsageGauge.Set(System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds);
                MemoryUsageGauge.Set(System.Diagnostics.Process.GetCurrentProcess().WorkingSet64);

                await next();
            });

            return app;
        }
    }
}