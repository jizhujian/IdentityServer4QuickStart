using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public async Task<IActionResult> CallWebApi()
        {
            var client = _httpClientFactory.CreateClient("client");
            var content = await client.GetStringAsync("identity/getuserclaims");
            var data = JsonSerializer.Deserialize<List<ClaimItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.UserClaims = data;
            return View("CallWebApi");
        }
    }
}