using IdentityModel.Client;
using IdentityModel.OidcClient;
using NativeConsoleClient;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var authenticationOptions = configuration.GetSection("Authentication").Get<AuthenticationOptions>();

var browser = new SystemBrowser();
var options = new OidcClientOptions
{
    Authority = authenticationOptions.Authority,
    ClientId = authenticationOptions.ClientId,
    ClientSecret = authenticationOptions.ClientSecret,
    RedirectUri = $"http://127.0.0.1:{browser.Port}",
    Scope = authenticationOptions.Scope,
    Browser = browser,
};
var oidcClient = new OidcClient(options);
var loginResult = await oidcClient.LoginAsync();

if (loginResult.IsError)
{
    Console.WriteLine("错误信息:{0}", loginResult.Error);
    return;
};

Console.WriteLine($"identity token = {loginResult.IdentityToken}");
Console.WriteLine($"name = {0}", loginResult.User.Identity?.Name);
Console.WriteLine($"access token = {loginResult.AccessToken}");
Console.WriteLine("access token expiration = {0}", loginResult.AccessTokenExpiration);
Console.WriteLine($"refresh token = {loginResult.RefreshToken ?? "none"}");

Console.WriteLine("****** Claims ******");
foreach (var claim in loginResult.User.Claims)
{
    Console.WriteLine("{0} = {1}", claim.Type, claim.Value);
};

var accessToken = loginResult.AccessToken;
var refreshToken = loginResult.RefreshToken;
if (!string.IsNullOrWhiteSpace(refreshToken))
{
    Console.WriteLine("****** Refresh Token ******");
    var refreshTokenResult = await oidcClient.RefreshTokenAsync(refreshToken);
    if (refreshTokenResult.IsError)
    {
        Console.WriteLine("错误信息:{0}", refreshTokenResult.Error);
    }
    else
    {
        accessToken = refreshTokenResult.AccessToken;
        refreshToken = refreshTokenResult.RefreshToken;
        Console.WriteLine($"access token = {accessToken}");
        Console.WriteLine("expires in = {0}", refreshTokenResult.ExpiresIn);
        Console.WriteLine("access token expiration = {0}", refreshTokenResult.AccessTokenExpiration);
        Console.WriteLine($"refresh token = {refreshToken ?? "none"}");
    }
};

Console.WriteLine("****** Call WebApi ******");
var client = new HttpClient();
client.SetBearerToken(accessToken);
var response = await client.GetAsync(configuration.GetValue<string>("ApiUrl"));
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(content);
}

Console.ReadLine();
