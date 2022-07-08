using IdentityModel.OidcClient.Browser;
using Microsoft.Web.WebView2.WinForms;

namespace WinFormsClient
{
    internal class WinFormsWebView2 : IBrowser
    {
        private BrowserOptions? _options;

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            _options = options;

            using var form = new Form
            {
                Name = "WebAuthentication",
                Text = "登录",
                Width = 1024,
                Height = 768,
                StartPosition = FormStartPosition.CenterScreen
            };
            using var webView = new WebView2()
            {
                Dock = DockStyle.Fill
            };
            var semaphoreSlim = new SemaphoreSlim(0, 1);

            var browserResult = new BrowserResult
            {
                ResultType = BrowserResultType.UserCancel
            };

            form.FormClosed += (_, _) =>
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
                form.Controls.Add(webView);
                webView.Show();

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
                webView.Hide();
            }

            return browserResult;
        }

        private bool IsBrowserNavigatingToRedirectUri(Uri uri)
        {
            return uri.AbsoluteUri.StartsWith(_options!.EndUrl);
        }
    }
}
