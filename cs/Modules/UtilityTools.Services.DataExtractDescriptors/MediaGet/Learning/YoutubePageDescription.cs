using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;
using UtilityTools.Services.Interfaces.Youtube;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class YoutubePageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        public string ShortIcon => "website.png";

        private readonly IYoutubeService youtubeService;

        private static string PREVIOUSURL = "";
        private static string NEXTTOKEN = "";
        private static string PREVIOUSTOKEN = "";

        public YoutubePageDescription(IYoutubeService youtubeService)
        {
            this.youtubeService = youtubeService;
        }

        public override string GetNextAddress(string address,out int page)
        {
            page = 0;
            if (!string.IsNullOrEmpty(NEXTTOKEN))
            {
                var url =  Regex.Replace(PREVIOUSURL, @"pageToken=(?<token>.*)", (mc) =>
                {
                    return $"pageToken={NEXTTOKEN}";
                });

                if(url == PREVIOUSURL)
                {
                    url = url + $"&pageToken={NEXTTOKEN}";
                }

                return url;
            }

            return string.Empty;
        }

        public override string GetProceedingAddress(string address,out int page)
        {
            page = 0;
            if (!string.IsNullOrEmpty(PREVIOUSTOKEN))
            {
                return Regex.Replace(PREVIOUSURL, @"pageToken=(?<token>.*)", (mc) =>
                {
                    return $"pageToken={PREVIOUSTOKEN}";
                });
            }

            return string.Empty;
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            var response = await youtubeService.GetVideoListByUrlAsync(MediaGetContext.Key);

            PREVIOUSURL = MediaGetContext.Key;

            if (!string.IsNullOrEmpty(response.nextPageToken))
            {
                NEXTTOKEN = response.nextPageToken;
            }

            if (!string.IsNullOrEmpty(response.prevPageToken))
            {
                PREVIOUSTOKEN = response.prevPageToken;
            }

            int index = 0;
            foreach (var item in response.items)
            {
                data.Add(new MediaDataDescription
                {
                    Name = HttpUtility.UrlDecode(item.snippet.title).Replace("&amp;","&"),
                    Url = $"https://www.youtube.com/watch?v={item.id.videoId}",
                    MediaType = MediaSymbolType.Youtube,
                    Order = index,
                    ImageUrl = item.snippet.thumbnails.medium.url
                });
                index++;
            }

            return Result(data);
        }

    }
}
