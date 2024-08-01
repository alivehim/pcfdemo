using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Youtube
{

    public class YoutubeVideoResponse
    {
        public string nextPageToken { get; set; }

        public string prevPageToken { get; set; }
        public IList<YoutubeVideoItem> items { get; set; }
    }

    public class YoutubeVideoItem
    {
        public YoutubeVideoSnippet snippet { get; set; }

        public YoutubeId id { get; set; }
    }

    public class YoutubeVideoSnippet
    {
        public string title { get; set; }
        public string description { get; set; }

        public YoutebeVideoThunmnails thumbnails { get; set; }
    }

    public class YoutubeId
    {
        public string videoId { get; set; }
    }

    public class YoutebeVideoThunmnails
    {
        public YoutebeVideoThunmnailInfo @default { get; set; }
        public YoutebeVideoThunmnailInfo @medium { get; set; }
        public YoutebeVideoThunmnailInfo @high { get; set; }
    }
    public class YoutebeVideoThunmnailInfo
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
