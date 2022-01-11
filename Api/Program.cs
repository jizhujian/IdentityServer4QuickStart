using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

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
});

var app = builder.Build();

app.UseCors("*");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.OAuthClientId(swaggerUIOptions.ClientId);
    options.OAuthAppName(swaggerUIOptions.ApiName);
    options.OAuthUsePkce();
});

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
