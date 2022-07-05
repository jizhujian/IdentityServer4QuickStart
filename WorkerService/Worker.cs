using Microsoft.Extensions.Hosting;

namespace WorkerService
{
    internal class Worker : BackgroundService
    {
        private readonly IHttpClientFactory _clientFactory;

        public Worker(IHttpClientFactory factory)
        {
            _clientFactory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(2000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");

                var client = _clientFactory.CreateClient("client");
                var response = await client.GetAsync("identity/getuserclaims", stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(stoppingToken);
                    Console.WriteLine($"API response: {content}");
                }
                else
                {
                    Console.WriteLine($"API returned: {response.StatusCode}");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
