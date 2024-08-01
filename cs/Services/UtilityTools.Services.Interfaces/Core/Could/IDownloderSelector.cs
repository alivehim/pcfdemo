using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Core.Could.Download;

namespace UtilityTools.Services.Interfaces.Core.Could
{
    public interface IDownloderSelector
    {
        IRPCDownloader GetDownloader();

        void Return(IRPCDownloader downloader);
    }
}
