using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var authenticationOptions = builder.Configuration.GetSection("Authentication").Get<Api.AuthenticationOptions>();
var swaggerUIOptions = builder.Configuration.GetSection("SwaggerUI").Get<Api.SwaggerUIOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = authenticationOptions.Authority;
        options.RequireHttpsMetadata = authenticationOptions.RequireHttpsMetadata;
        options.Audience = authenticationOptions.Scope;
    });

builder.Services.AddCors(o => o.AddPolicy("*", builder => builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowCredentials()));

builder.Services.AddControllers();
if (swaggerUIOptions.Enabled)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{authenticationOptions.Authority}/connect/authorize"),
                    TokenUrl = new Uri($"{authenticationOptions.Authority}/connect/token"),
                    Scopes = new Dictionary<string, string> { { authenticationOptions.Scope, swaggerUIOptions.ApiName } }
                }
            }
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } },
            new[] { authenticationOptions.Scope  }
        }
        });
        options.SwaggerDoc(swaggerUIOptions.ApiVersion, new OpenApiInfo { Title = swaggerUIOptions.ApiName });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath, true);
    });
};

var app = builder.Build();

var forwardingOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.All
};
forwardingOptions.KnownNetworks.Clear();
forwardingOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardingOptions);

app.UseCors("*");

if (swaggerUIOptions.Enabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(swaggerUIOptions.ClientId);
        options.OAuthAppName(swaggerUIOptions.ApiName);
        options.OAuthClientSecret(swaggerUIOptions.ClientSecret);
    });
};

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();
