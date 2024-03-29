﻿using ConsoleClient;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var authenticationOptions = configuration.GetSection("Authentication").Get<AuthenticationOptions>();

// discover endpoints from metadata
var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
{
    Address = authenticationOptions.Authority,
    Policy = { RequireHttps = authenticationOptions.RequireHttpsMetadata }
});
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    Console.ReadLine();
    return;
}

// request token
var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = authenticationOptions.ClientId,
    ClientSecret = authenticationOptions.ClientSecret,
    UserName = authenticationOptions.UserName,
    Password = authenticationOptions.Password
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.ReadLine();
    return;
}

Console.WriteLine($"access_token = {tokenResponse.AccessToken}");
Console.WriteLine($"refresh_token = {tokenResponse.RefreshToken}");
Console.WriteLine($"token_type = {tokenResponse.TokenType}");
Console.WriteLine($"expires_in = {tokenResponse.ExpiresIn}");
Console.WriteLine($"scope = {tokenResponse.Scope}");

var userInfo = await client.GetUserInfoAsync(new UserInfoRequest
{
    Address = disco.UserInfoEndpoint,
    Token = tokenResponse.AccessToken
});
if (userInfo.IsError)
{
    Console.WriteLine(userInfo.Error);
    Console.ReadLine();
    return;
};
foreach (var claim in userInfo.Claims)
{
    Console.WriteLine($"{claim.Type} = {claim.Value}");
};

// call api
client.SetBearerToken(tokenResponse.AccessToken);

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
