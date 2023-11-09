using Api.Utils;
using Microsoft.AspNetCore.Hosting;
using Polly;
using ShoppingCart.Abstractions;
using ShoppingCart.ShoppingCart;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
builder.Build().Run();