using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMqSample.Publisher.Data;
using RabbitMqSample.Publisher.Model;
using RabbitMqSample.Shared;

namespace RabbitMqSample.Publisher.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EventController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<EventController> logger;

        public EventController(ILogger<EventController> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        
        [HttpGet]
        public async Task<IActionResult> FireEvent()
        {
            var itemCreatedEvent = new ItemCreatedEvent
            {
                Name = "Test Item"
            };

            var outBoxMessage = new OutboxMessage
            {
                EventPayload = JsonConvert.SerializeObject(itemCreatedEvent),
                EventType = itemCreatedEvent.GetType().AssemblyQualifiedName
            };

            await context.OutboxMessages.AddAsync(outBoxMessage);
            await context.SaveChangesAsync();

            logger.LogInformation("Event published successfully");

            return Ok();
        }
    }
}