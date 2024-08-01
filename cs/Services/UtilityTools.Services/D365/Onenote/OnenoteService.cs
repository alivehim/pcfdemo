using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Aspect;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.D365;
using UtilityTools.Services.Aspects;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Services.D365
{
    public class OnenoteService : IGraphOnenoteService
    {

        [LogAspect("GetOnenoteBooks")]
        public async Task<IList<BookItem>> GetOnenoteBooksAsync()
        {
            string address = "https://www.onenote.com/v1.0/me/onenote/notebooks";

            var result = await HttpHelper.GetDynamic365ContentAsync(address, Encoding.UTF8, Settings.Current.OnenoteToken);

            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<BookResponse>(result);


            return json.value;
        }

        [CacheAspect("OnenoteSection")]
        public async Task<IList<SectionItem>> GetOnenoteSectionsAsync(string url)
        {

            var result = await HttpHelper.GetDynamic365ContentAsync(url, Encoding.UTF8, Settings.Current.OnenoteToken);

            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionResponse>(result);


            return json.value;
        }

        public async Task<PageResponse> GetOnenotePagesAsync(string url)
        {

            var result = await HttpHelper.GetDynamic365ContentAsync(url, Encoding.UTF8, Settings.Current.OnenoteToken);

            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<PageResponse>(result);


            return json;
        }

        public async Task<string> GetOnenoteContentAsync(string url)
        {

            var result = await HttpHelper.GetDynamic365ContentAsync(url, Encoding.UTF8, Settings.Current.OnenoteToken);



            return result;
        }


        public async Task<SectionItem> CreateSectionAsync(string url, string sectionName)
        {
            //url = "https://graph.microsoft.com/v1.0/users/8c7f11f5-2cec-4b01-a363-155fe5b8f457/onenote/notebooks/1-cabcf8ab-a091-470b-97dd-2a928dc2db93/sections";

            if (sectionName.Length > 50)
            {
                sectionName = sectionName.Substring(0, 50);
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                displayName = sectionName
            });


            var result = await HttpHelper.PostDataToUrl(url, json, Settings.Current.OnenoteToken);

            var sectionItem = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionItem>(result);


            return sectionItem;
        }

        public async Task CreatePageAsync(string sectionUrl, string title, string content)
        {
            var html = @$"<!DOCTYPE html>
<html>
  <head>
    <title>{title}</title>
    <meta name=""created"" content=""{DateTime.Now.ToString()}"" />
  </head>
  <body>
    {content.Replace("’", "'")}
  </body>
</html>";
            await HttpHelper.PostDataToUrlWithText(sectionUrl, html, Settings.Current.OnenoteToken);
        }
    }
}
