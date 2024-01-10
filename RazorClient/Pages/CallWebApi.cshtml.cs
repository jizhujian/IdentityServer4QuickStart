using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RazorClient.Pages
{
    public class CallWebApiModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CallWebApiModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ClaimItem>? UserClaims { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("client");
            var content = await client.GetStringAsync("identity/getuserclaims");
            UserClaims = JsonSerializer.Deserialize<List<ClaimItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
