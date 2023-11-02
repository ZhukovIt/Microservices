using Microsoft.EntityFrameworkCore;
using ShoppingCart.DomainEvents;

namespace Microservices.DomainEvents
{
    public class EventDBContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
    }
}
