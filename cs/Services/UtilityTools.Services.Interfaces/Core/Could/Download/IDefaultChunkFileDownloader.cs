using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Core.Could.Download
{
    public interface IDefaultChunkFileDownloader
    {
        int ErrorNum { get; }

        void DownloadFileChunks(IStreamUXItemDescription item, CancellationToken cancellationToken);
    }
}
