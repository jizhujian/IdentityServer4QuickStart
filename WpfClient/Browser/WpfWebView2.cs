using IdentityModel.OidcClient.Browser;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfClient
{
    internal class WpfWebView2 : IBrowser
    {
        private BrowserOptions? _options;

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            _options = options;

            var form = new Window()
            {
                Width = 1024,
                Height = 768,
                Title = "登录",
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            using var webView = new WebView2();

            var semaphoreSlim = new SemaphoreSlim(0, 1);

            var browserResult = new BrowserResult()
            {
                ResultType = BrowserResultType.UserCancel
            };

            form.Closing += (_, _) =>
            {
                semaphoreSlim.Release();
            };

            webView.NavigationStarting += (_, e) =>
            {
                if (IsBrowserNavigatingToRedirectUri(new Uri(e.Uri)))
                {
                    e.Cancel = true;

                    browserResult = new BrowserResult()
                    {
                        ResultType = BrowserResultType.Success,
                        Response = new Uri(e.Uri).AbsoluteUri
                    };

                    semaphoreSlim.Release();
                    form.Close();
                }
            };

            try
            {
                form.Content = webView;
                form.Show();

                // Initialization
                await webView.EnsureCoreWebView2Async(null);

                // Delete existing Cookies so previous logins won't remembered
                webView.CoreWebView2.CookieManager.DeleteAllCookies();

                // Navigate
                webView.CoreWebView2.Navigate(_options.StartUrl);

                await semaphoreSlim.WaitAsync();
            }
            finally
            {
                form.Hide();
            }

            return browserResult;
        }

        private bool IsBrowserNavigatingToRedirectUri(Uri uri)
        {
            return uri.AbsoluteUri.StartsWith(_options!.EndUrl);
        }
    }
}
