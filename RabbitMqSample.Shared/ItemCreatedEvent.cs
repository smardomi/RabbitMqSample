namespace RabbitMqSample.Shared
{
    public class ItemCreatedEvent : IntegrationBaseEvent
    {
        public string Name { get; set; }
    }
}
