using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Unsplash.Core.DataAccess;
using System.IO;

namespace Unsplash.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                         .Build();
            //Read Configuration from appSettings
            //remove log to console when in production
            //string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config =  new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .Build();
            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<Core.DataAccess.UnsplashContext>();

                    //4. Call the DataGenerator to create sample data
                    //await ModelBuilderExtensions.Initialize(services);
                    //logging initial app state
                    Log.Information("Unsplash just started.");
                    host.Run();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "The Application failed to start.");

                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
