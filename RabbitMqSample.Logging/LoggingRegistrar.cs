using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace RabbitMqSample.Logging
{
    public static class LoggingRegistrar
    {
        public static IHostBuilder RegisterLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((hostContext, services, configuration) =>
            {

                configuration
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        IndexFormat = "RabbitMqSample-logs-{0:yyyy.MM.dd}",
                        MinimumLogEventLevel = LogEventLevel.Information,
                        BatchPostingLimit = 50,
                        Period = TimeSpan.FromSeconds(2),
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
                        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                        CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                        BufferBaseFilename = "./logs/buffer"
                    });
            });

            return hostBuilder;
        }
    }
}