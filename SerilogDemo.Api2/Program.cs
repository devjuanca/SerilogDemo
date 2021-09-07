using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.MSSqlServer;
using SerilogDemo.Api2.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SerilogDemo.Api2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                var currentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();

                

                Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)

                    .WriteTo.Console(
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] [{EventId}] {Message}{Properties}{NewLine}{Exception}"
                        )
                    .Enrich.With(new CustomEnricher(currentUserService))
                    .Enrich.WithAssemblyName()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()


                     .WriteTo.File(
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] [{EventId}] {Message}{Properties}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    path: Path.Combine("C:\\Desarrollos .NET\\Proyectos\\Demos\\SerilogDemo\\Api1\\SerilogDemo.Api1\\SerilogDemo.Api2\\logs", "logFile.txt")
                    )
                    .Enrich.With(new CustomEnricher(currentUserService))
                    .Enrich.WithAssemblyName()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()


                    .WriteTo.MSSqlServer(
                        connectionString: configuration.GetConnectionString("Default"),
                        sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents", AutoCreateSqlTable = true }
                        )
                    .Enrich.With(new CustomEnricher(currentUserService))
                    .Enrich.WithAssemblyName()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()


                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                        node: new Uri(configuration.GetValue<string>("LoggingOptions:NodeUrl")))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat = $"Global-Logs-{DateTime.Now:dd-mm-yyyy}"

                    })
                    .Enrich.With(new CustomEnricher(currentUserService))
                    .Enrich.WithAssemblyName()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()

                    .WriteTo.Seq(serverUrl: "http://localhost:5341/", apiKey: "Api2-Traffic")
                    .Enrich.With(new CustomEnricher(currentUserService))
                    .Enrich.WithAssemblyName()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()



                    .CreateLogger();



            }

            await host.RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
