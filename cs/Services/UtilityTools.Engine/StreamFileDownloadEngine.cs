using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;

namespace UtilityTools.Engine
{
    public class StreamFileDownloadEngine : IStreamFileDownloadEngine
    {
        public Task Run(IStreamUXItemDescription item)
        {
            return Task.Run(async () =>
            {
                try
                {
                    IProgress<float> progress = new Progress<float>(s =>
                    {
                        item.Progress = (int)(s * 100);
                    });

                    await HttpHelper.DownloadFileAsync(item.StreamUri, item.StoragePath, progress,item.CancellationTokenSource.Token);
                    item.TaskStage = TaskStage.Done;

                }
                catch (Exception ex)
                {
                    item.Error(ex.ToString());
                    item.TaskStage = TaskStage.Error;
                }
                finally
                {
                    item.IsWaiting = false;
                    item.IsDownloading = false;
                }

            });

        }
    }
}
