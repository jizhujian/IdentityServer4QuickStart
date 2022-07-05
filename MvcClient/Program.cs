using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var authenticationOptions = builder.Configuration.GetSection("Authentication").Get<MvcClient.AuthenticationOptions>();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies", options =>
    {
        options.Events.OnSigningOut = async e =>
        {
            await e.HttpContext.RevokeUserRefreshTokenAsync();
        };
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = authenticationOptions.Authority;
        options.RequireHttpsMetadata = authenticationOptions.RequireHttpsMetadata;
        options.Scope.Clear();
        foreach (var scope in authenticationOptions.Scopes)
        {
            options.Scope.Add(scope);
        }
        options.Scope.Add("offline_access");
        options.ClientId = authenticationOptions.ClientId;
        options.ClientSecret = authenticationOptions.ClientSecret;
        options.ResponseType = "code";
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
    });

builder.Services.AddAccessTokenManagement();

builder.Services.AddUserAccessTokenHttpClient("client", configureClient: client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseUrl"));
});
//builder.Services.AddClientAccessTokenHttpClient("client", configureClient: client =>
//{
//    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseUrl"));
//});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .RequireAuthorization();

app.Run();
