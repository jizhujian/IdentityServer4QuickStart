using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (await LoginHelper.LoginAsync())
            {
                var sb = new StringBuilder();
                sb.AppendLine($"identity token = {LoginHelper.LoginResult?.IdentityToken}");
                sb.AppendLine($"name = {LoginHelper.LoginResult!.User.Identity?.Name}");
                sb.AppendLine($"access token = {LoginHelper.AccessToken}");
                sb.AppendLine($"access token expiration = {LoginHelper.LoginResult?.AccessTokenExpiration}");
                sb.AppendLine($"refresh token = {LoginHelper.RefreshToken ?? "none"}");

                sb.AppendLine("****** Claims ******");
                foreach (var claim in LoginHelper.LoginResult!.User.Claims)
                {
                    sb.AppendLine($"{claim.Type} = {claim.Value}");
                };
                Output.Text = sb.ToString();
            }
            else
            {
                Close();
            };
        }

        private async void RefreshTokenButton_Click(object sender, RoutedEventArgs e)
        {
            if (await LoginHelper.RefreshTokenAsync())
            {
                var sb = new StringBuilder();
                sb.AppendLine($"access token = {LoginHelper.AccessToken}");
                sb.AppendLine($"expires in = {LoginHelper.RefreshTokenResult?.ExpiresIn}");
                sb.AppendLine($"access token expiration = {LoginHelper.RefreshTokenResult?.AccessTokenExpiration}");
                sb.AppendLine($"refresh token = {LoginHelper.RefreshToken ?? "none"}");
                Output.Text = sb.ToString();
            }
            else
            {
                Output.Text = $"错误信息: {LoginHelper.RefreshTokenResult?.Error}";
            };
        }

        private async void CallWebApiButton_Click(object sender, RoutedEventArgs e)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            var sb = new StringBuilder();
            var client = new HttpClient();
            client.SetBearerToken(LoginHelper.AccessToken);
            var response = await client.GetAsync(configuration.GetValue<string>("ApiUrl"));
            if (!response.IsSuccessStatusCode)
            {
                sb.AppendLine(response.StatusCode.ToString());
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                sb.AppendLine(content);
            }
            Output.Text = sb.ToString();
        }

    }
}
