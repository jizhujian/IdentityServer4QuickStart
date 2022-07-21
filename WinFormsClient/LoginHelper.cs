using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;

namespace WinFormsClient
{
    internal static class LoginHelper
    {
        private static readonly OidcClient _oidcClient;
        public static LoginResult? LoginResult;
        public static RefreshTokenResult? RefreshTokenResult;
        public static string? AccessToken;
        public static string? RefreshToken;

        static LoginHelper()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var authenticationOptions = configuration.GetSection("Authentication").Get<AuthenticationOptions>();

            var options = new OidcClientOptions
            {
                Authority = authenticationOptions.Authority,
                ClientId = authenticationOptions.ClientId,
                ClientSecret = authenticationOptions.ClientSecret,
                RedirectUri = $"http://127.0.0.1:{GetRandomUnusedPort()}",
                Scope = authenticationOptions.Scope,
                Browser = configuration.GetValue<bool>("WebView") ? new WinFormsWebView2() : new WinFormsWebView(),
            };

            _oidcClient = new OidcClient(options);
        }

        public async static Task<bool> LoginAsync()
        {
            LoginResult = await _oidcClient.LoginAsync();
            if (!LoginResult.IsError)
            {
                AccessToken = LoginResult.AccessToken;
                RefreshToken = LoginResult.RefreshToken;
            }

            return !LoginResult.IsError;
        }

        public async static Task<bool> RefreshTokenAsync()
        {
            RefreshTokenResult = await _oidcClient.RefreshTokenAsync(RefreshToken);
            if (!RefreshTokenResult.IsError)
            {
                AccessToken = RefreshTokenResult.AccessToken;
                RefreshToken = RefreshTokenResult.RefreshToken;
            }
            return !RefreshTokenResult.IsError;
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

    }
}
