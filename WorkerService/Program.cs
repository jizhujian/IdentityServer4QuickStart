using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using IdentityModel.Client;
using Serilog.Sinks.SystemConsole.Themes;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Debug()
                                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAccessTokenManagement(options =>
                    {
                        options.Client.Clients.Add("identityserver", new ClientCredentialsTokenRequest
                        {
                            Address = "https://jzj519576512.xicp.net:5000/connect/token",
                            ClientId = "IdentityServer4QuickStart_ConsoleApp",
                            ClientSecret = "Q!CTp2^6oxzhKunp",
                            Scope = "IdentityServer4QuickStart_api"
                        });
                    });

                    services.AddClientAccessTokenClient("client", configureClient: client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:50001/api/");
                    });

                    services.AddHostedService<Worker>();
                });

            return host;
        }
            
    }
}
