using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure.FFMPEG;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;

namespace UtilityTools.Engine
{
    public class VLDMeidaDonwloadEngine : ILivingStreamMediaDownloadEngine
    {
        /// <summary>
        /// https://freepctech.com/windows/m3u8-to-mp4/#:~:text=To%20convert%20M3U8%20to%20MP4%20with%20VLC%2C%20follow,Click%20on%20the%20%E2%80%9CConvert%2FSave%E2%80%9D%20button.%20...%20More%20items
        /// 
        /// ffmpeg -i http://…/playlist.m3u8 -c copy -bsf:a aac_adtstoasc output.mp4
        /// </summary>
        /// <param name="item"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task Run(IStreamUXItemDescription item)
        {
            FrapperWrapper frapperWrapper = new FrapperWrapper(new FFMPEG());


            var command = $@" -allowed_extensions ALL  -protocol_whitelist ""file,http,https,rtp,udp,tcp,tls,crypto"" -i ""{item.StreamUri}"" -c copy -bsf:a aac_adtstoasc ""{item.StoragePath}""";

            item.Info(command);
            frapperWrapper.ExecuteMultipleCommand(command,
                (obj, e) =>
                {
                    item.Info($"Receive {e.Data}");
                });


            item.TaskStage = TaskStage.Done;

            return Task.CompletedTask;
        }
    }
}
