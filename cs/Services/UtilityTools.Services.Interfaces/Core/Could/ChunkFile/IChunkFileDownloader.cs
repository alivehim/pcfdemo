using System.Threading;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces.Core.Could.ChunkFile
{
    public interface IChunkFileDownloader
    {
        void DownloadFileChunks(IStreamUXItemDescription item, string tempDirectory, CancellationToken cancellationToken);
    }
}