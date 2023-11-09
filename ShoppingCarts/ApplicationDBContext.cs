using Microsoft.EntityFrameworkCore;
using ShoppingCart.Events;

namespace Microservices.ShoppingCart
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {
            
        }

        public DbSet<Event> Events { get; set; }


    }
}
