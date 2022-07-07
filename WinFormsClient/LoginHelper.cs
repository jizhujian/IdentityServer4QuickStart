using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;

namespace WinFormsClient
{
    internal static class LoginHelper
    {
        private static OidcClient? _oidcClient;
        internal static LoginResult? LoginResult;
        internal static RefreshTokenResult? RefreshTokenResult;
        internal static string? AccessToken;
        internal static string? RefreshToken;

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
                Browser = new WinFormsWebView(),
            };

            _oidcClient = new OidcClient(options);
        }

        internal async static Task<bool> LoginAsync()
        {
            LoginResult = await _oidcClient!.LoginAsync();
            if (!LoginResult.IsError)
            {
                AccessToken = LoginHelper.LoginResult.AccessToken;
                RefreshToken = LoginHelper.LoginResult.RefreshToken;
            }

            return !LoginResult.IsError;
        }

        internal async static Task<bool> RefreshTokenAsync()
        {
            RefreshTokenResult = await _oidcClient!.RefreshTokenAsync(RefreshToken);
            if (!RefreshTokenResult.IsError)
            {
                AccessToken = RefreshTokenResult.AccessToken;
                RefreshToken = RefreshTokenResult.RefreshToken;
            }
            return !RefreshTokenResult.IsError;
        }

        internal async static Task LogoutAsync()
        {
            await _oidcClient!.LogoutAsync();
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
