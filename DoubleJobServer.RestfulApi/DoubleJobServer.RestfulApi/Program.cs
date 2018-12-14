using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DoubleJobServer.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DoubleJobServer.RestfulApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();

            Console.Title = "MY Restful Api";

            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var salesContext = services.GetRequiredService<MyContext>();
                    MyContextSeed.SeedAsync(salesContext, loggerFactory).Wait();
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseIISIntegration()   //让web宿主工作于IIS之后需要使用IWebHostBuilder的UseIISIntegration这个扩展方法.
            .UseUrls("http://0.0.0.0:5000")
            .UseStartup<Startup>()
            .UseSerilog();
    }
}
