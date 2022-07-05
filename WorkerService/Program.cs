using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var authenticationOptions = configuration.GetSection("Authentication").Get<AuthenticationOptions>();

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddClientAccessTokenManagement(options =>
    {
        options.Clients.Add("identityserver", new ClientCredentialsTokenRequest
        {
            Address = authenticationOptions.Authority + "/connect/token",
            ClientId = authenticationOptions.ClientId,
            ClientSecret = authenticationOptions.ClientSecret
        });
    });

    services.AddClientAccessTokenHttpClient("client", configureClient: client =>
    {
        client.BaseAddress = new Uri(configuration.GetValue<string>("ApiBaseUrl"));
    });

    services.AddHostedService<Worker>();
});

var app = builder.Build();

app.Run();
