using MassTransit;
using Newtonsoft.Json;
using RabbitMqSample.Publisher.Data;

namespace RabbitMqSample.Publisher.ServiceWorkers
{
    public class OutBoxServiceWorker : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public OutBoxServiceWorker(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishOutboxMessages(stoppingToken);
            }
        }

        private async Task PublishOutboxMessages(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                await using var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var messages = appDbContext.OutboxMessages.Where(om => om.ExpireDate == null
                                                                                                || om.ExpireDate >= DateTime.Now)
                                                                                  .ToList();

                foreach (var outboxMessage in messages)
                {
                    try
                    {
                        var messageType = Type.GetType(outboxMessage.EventType);
                        var message = JsonConvert.DeserializeObject(outboxMessage.EventPayload, messageType!);

                        await bus.Publish(message, messageType, stoppingToken);

                        appDbContext.OutboxMessages.Remove(outboxMessage);
                        await appDbContext.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
