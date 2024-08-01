using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.Youtube;
using UtilityTools.Services.Interfaces.Youtube;

namespace UtilityTools.Services.Google
{
    public class YoutubeService : IYoutubeService
    {
        private string KEY = "AIzaSyCcXpLNJeLSkcvDfiHR7awzStOBRvGSNjg";

        public async Task<IList<YoutubeVideoItem>> GetVideoListAsync(string channelId)
        {
            string url = $@"https://youtube.googleapis.com/youtube/v3/search?part=snippet&channelId={channelId}&order=date&type=video&key={KEY}&maxResults=20";
            var result = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8);

            if (!string.IsNullOrEmpty(result))
            {

                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<YoutubeVideoResponse>(result);

                return jsonObj.items;
            }

            return null;
        }

        public async Task<YoutubeVideoResponse> GetVideoListByUrlAsync(string url)
        {
            var result = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8);

            if (!string.IsNullOrEmpty(result))
            {

                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<YoutubeVideoResponse>(result);

                return jsonObj;
            }

            return null;
        }
    }
}
