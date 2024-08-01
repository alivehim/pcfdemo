using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using UtilityTools.Services.Interfaces.Core.Could;
using UtilityTools.Services.Interfaces.Core.Could.Download;

namespace UtilityTools.Engine
{
    public class DownloderSelector : IDownloderSelector
    {
        private readonly IDownloadProvider downloadProvider;

        private ObjectPool<IRPCDownloader> queue;

        public DownloderSelector(IDownloadProvider downloadProvider)
        {
            this.downloadProvider = downloadProvider;
            //queue = new ObjectPool<IRPCDownloader> (this.downloadProvider.NormalDownloads);
            queue = new ObjectPool<IRPCDownloader> (this.downloadProvider.SpecialDownloads);

        }

        public IRPCDownloader GetDownloader()
        {
            var result= queue.Get();
            Console.Write(result.Address);
            return result;  
        }

        public void Return(IRPCDownloader downloader)
        {
            queue.Put(downloader);
        }
    }
}
