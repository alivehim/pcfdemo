using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityTools.Engine.Extensions
{
    public static class WebClientExtensions
    {
        public static async Task<string> DownloadStringTaskAsync(this WebClient webClient, string url, CancellationToken cancellationToken)
        {
            using (cancellationToken.Register(webClient.CancelAsync))
            {
                return await webClient.DownloadStringTaskAsync(url);
            }
        }

        public static async Task<string> DownloadStringTaskAsync(this WebClient webClient, Uri uri, CancellationToken cancellationToken)
        {
            using (cancellationToken.Register(webClient.CancelAsync))
            {
                return await webClient.DownloadStringTaskAsync(uri);
            }
        }
    }
}
