using IdentityModel.OidcClient.Browser;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfClient
{
    internal class WpfWebView : IBrowser
    {

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var form = new Window()
            {
                Width = 1024,
                Height = 768,
                Title = "登录",
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            // Note: Unfortunately, WebBrowser is very limited and does not give sufficient information for 
            //   robust error handling. The alternative is to use a system browser or third party embedded
            //   library (which tend to balloon the size of your application and are complicated).
            var webBrowser = new WebBrowser();

            var signal = new SemaphoreSlim(0, 1);

            var result = new BrowserResult()
            {
                ResultType = BrowserResultType.UserCancel
            };

            form.Closed += (_, _) =>
            {
                signal.Release();
            };

            webBrowser.Navigating += (_, e) =>
            {
                if (e.Uri.AbsoluteUri.StartsWith(options.EndUrl))
                {
                    e.Cancel = true;

                    result = new BrowserResult()
                    {
                        ResultType = BrowserResultType.Success,
                        Response = e.Uri.AbsoluteUri
                    };

                    signal.Release();

                    form.Close();
                }
            };

            form.Content = webBrowser;
            form.Show();
            webBrowser.Navigate(options.StartUrl);

            await signal.WaitAsync();

            return result;
        }
    }
}
