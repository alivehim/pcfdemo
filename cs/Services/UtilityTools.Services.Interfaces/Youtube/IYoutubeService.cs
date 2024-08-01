using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.Youtube;

namespace UtilityTools.Services.Interfaces.Youtube
{
    public interface IYoutubeService
    {
        Task<IList<YoutubeVideoItem>> GetVideoListAsync(string channelId);

        Task<YoutubeVideoResponse> GetVideoListByUrlAsync(string url);

    }
}
