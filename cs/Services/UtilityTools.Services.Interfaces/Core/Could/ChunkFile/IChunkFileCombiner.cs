using System.Collections.Generic;
using System.IO;
using System.Threading;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces.Core.Could.ChunkFile
{
    public interface IChunkFileCombiner
    {
        IEnumerable<FileInfo> GetChunkFileInfos(string chunkFileDirectory);
        FileInfo CombineChunkFiles(IStreamUXItemDescription item, IEnumerable<FileInfo> chunkFiles, string outputFilePath, CancellationToken cancellationToken);
    }
}