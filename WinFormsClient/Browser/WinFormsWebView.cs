using IdentityModel.OidcClient.Browser;

namespace WinFormsClient
{
    internal class WinFormsWebView : IBrowser
    {

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken token = default)
        {
            using (var form = new Form
            {
                Name = "WebAuthentication",
                Text = "登录",
                Width = 1027,
                Height = 768,
                StartPosition = FormStartPosition.CenterScreen
            })
            using (var browser = new ExtendedWebBrowser()
            {
                Dock = DockStyle.Fill
            })
            {
                var signal = new SemaphoreSlim(0, 1);

                var result = new BrowserResult
                {
                    ResultType = BrowserResultType.UserCancel
                };

                form.FormClosed += (o, e) =>
                {
                    signal.Release();
                };

                browser.NavigateError += (o, e) =>
                {
                    if (e.Url.StartsWith(options.EndUrl))
                    {
                        e.Cancel = true;
                        result.ResultType = BrowserResultType.Success;
                        result.Response = e.Url;
                        signal.Release();
                    }
                };

                browser.DocumentCompleted += (o, e) =>
                {
                    if (e.Url!.AbsoluteUri.StartsWith(options.EndUrl))
                    {
                        result.ResultType = BrowserResultType.Success;
                        result.Response = e.Url.AbsoluteUri;
                        signal.Release();
                    }
                };

                try
                {
                    form.Controls.Add(browser);
                    browser.Show();

                    form.Show();
                    browser.Navigate(options.StartUrl);

                    await signal.WaitAsync();
                }
                finally
                {
                    form.Hide();
                    browser.Hide();
                }

                return result;
            }
        }

    }
}
