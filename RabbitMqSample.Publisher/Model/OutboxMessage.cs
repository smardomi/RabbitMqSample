using System.ComponentModel.DataAnnotations;

namespace RabbitMqSample.Publisher.Model
{
    public class OutboxMessage
    {
        public OutboxMessage()
        {
            EventDate = DateTime.UtcNow;
        }


        [Key]
        public int Id { get; set; }
        public string EventType { get; set; }
        public string EventPayload { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
