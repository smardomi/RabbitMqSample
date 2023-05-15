using MassTransit;
using RabbitMqSample.Shared;

namespace RabbitMqSample.Consumer.Consumers
{
    public class ItemCreatedEventConsumer : IConsumer<ItemCreatedEvent>
    {
        private readonly ILogger<ItemCreatedEventConsumer> logger;

        public ItemCreatedEventConsumer(ILogger<ItemCreatedEventConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<ItemCreatedEvent> context)
        {
            //throw new Exception("خطا خطا خطا");

            logger.LogInformation("Event consumed successfully.");

            //File.AppendAllText(@"C:\Users\saeedmardomi\Desktop\tst.txt", "Hello and Welcome" + Environment.NewLine);

            return Task.CompletedTask;
        }
    }
}
