using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
  class Program
  {
    static async Task Main()
    {
      // discover endpoints from metadata
      var client = new HttpClient();
      var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
      {
        Address = "http://jzj519576512.xicp.net:5000",
        Policy =
        {
            RequireHttps = false
        }
      });
      if (disco.IsError)
      {
        Console.WriteLine(disco.Error);
        return;
      }

      // request token
      var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
      {
        Address = disco.TokenEndpoint,
        ClientId = "IdentityServer4QuickStart_ConsoleApp",
        ClientSecret = "Q!CTp2^6oxzhKunp",
        Scope = "IdentityServer4QuickStart_api"
      });

      if (tokenResponse.IsError)
      {
        Console.WriteLine(tokenResponse.Error);
        return;
      }

      Console.WriteLine(tokenResponse.Json);

      // call api
      client.SetBearerToken(tokenResponse.AccessToken);

      var response = await client.GetAsync("http://localhost:50001/api/identity/getuserclaims");
      if (!response.IsSuccessStatusCode)
      {
        Console.WriteLine(response.StatusCode);
      }
      else
      {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(JArray.Parse(content));
      }
    }
  }
}
