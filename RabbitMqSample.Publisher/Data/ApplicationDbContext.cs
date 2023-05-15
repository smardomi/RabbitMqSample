using Microsoft.EntityFrameworkCore;
using RabbitMqSample.Publisher.Model;

namespace RabbitMqSample.Publisher.Data
{
    public class ApplicationDbContext : DbContext  
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}
