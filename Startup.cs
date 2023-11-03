using Microservices.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using Polly;
using ShoppingCart.Abstractions;
using ShoppingCart.ShoppingCart;
using System.Diagnostics;
using System.Linq;

namespace Microservices
{
    public class Startup
    {
        public Startup(IConfiguration config) 
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));
            

            services.Scan(selector => selector
        .FromAssemblyOf<Startup>()
        .AddClasses(c => c.Where(t => t != typeof(ProductCatalogClient) && t.GetMethods().All(m => m.Name != "<Clone>$")))
        .AsImplementedInterfaces());
            services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>()
              .AddTransientHttpErrorPolicy(p =>
                p.WaitAndRetryAsync(
                  3,
                  attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt))));
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => await context.Response.WriteAsync("Server is launch!"));
                endpoints.MapControllers();
            });
        }
    }
}
