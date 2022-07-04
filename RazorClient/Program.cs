using RazorClient;

var builder = WebApplication.CreateBuilder(args);

var authenticationOptions = builder.Configuration.GetSection("Authentication").Get<AuthenticationOptions>();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
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

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages()
    .RequireAuthorization();

app.Run();
