using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Engine.Download;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;
using static NetMQ.NetMQSelector;

namespace UtilityTools.Engine
{
    public class FakeLivingMediaDownloadEngine : ILivingStreamMediaDownloadEngine
    {
        public Task Run(IStreamUXItemDescription item)
        {
            item.NumberOfChunkFiles = 100;
            Thread.Sleep(2000);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                //this.bW_ProgressChanged(sender, new ProgressChangedEventArgs(i, null));

                if (!DowloadThread.RunTry(() =>
                {
                    if (!item.CancellationTokenSource.Token.IsCancellationRequested)
                    {
                        item.CompletedNumber++;

                        item.Progress = i;

                        return true;
                    }

                    return false;

                }, item, (item, ex, count) =>
                {

                }))
                {
                    break;
                }


               


            }
            item.IsDownloading = false;
            item.TaskStage = TaskStage.Done;
            return Task.CompletedTask;
        }
    }
}
