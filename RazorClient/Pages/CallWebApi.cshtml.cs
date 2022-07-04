using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RazorClient.Pages
{
    public class CallWebApiModel : PageModel
    {
        public List<ClaimItem>? UserClaims { get; set; }

        public async Task OnGetAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:39999/api/identity/getuserclaims");
            UserClaims = JsonSerializer.Deserialize<List<ClaimItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
