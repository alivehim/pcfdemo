using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces.Core.Could.ChunkFile
{
    public interface IChunkFileProvider
    {
        //Queue<string> GetX(IStreamUXItemDescription item, CancellationToken token);
        Task<Queue<string>> GetChunkFiles(IStreamUXItemDescription item, CancellationToken token);
    }
}