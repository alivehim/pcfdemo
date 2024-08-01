using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure
{
    public interface IHttpService
    {
        Task<string> GetUrlContentAsync(string uriString, Encoding encoding);

        Task<string> GetUrlContentAsync(string uriString, Encoding encoding, string Referer);
        //Task<string> GetPageSourceAsync(string uriString, out CookieContainer cc);
        Task<(string, CookieContainer)> GetPageSourceAsync(string uriString);
        Task<string> GetHtmlSourceAsync(string uriString);
    }
}
