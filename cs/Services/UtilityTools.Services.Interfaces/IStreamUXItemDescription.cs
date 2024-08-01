using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Services.Interfaces
{
    public interface IStreamUXItemDescription : IBaseUXItemDescription
    {
        bool IsLivingStream { get; set; }
        string Url { get; set; }
        MediaSymbolType SymbolType { get; set; }
        string StreamUri { get; set; }
        string Name { get; set; }
        /// <summary>
        /// Gets or Sets the number of chunk files
        /// </summary>
        int NumberOfChunkFiles { get; set; }

        //double ChunkProgressPosition { get; set; }
        //int ChunkDownloadPosition { get; set; }
        //double ChunkCombinePosition { get; set; }
        int CompletedNumber { get; set; }
        int Progress
        {
            get;
            set;
        }

        Queue<string> ParsedChunks { get; set; }


        string ExtensionName
        {
            get; set;
        }

        string TempDirectory
        {
            get; set;
        }

        string CombinedFile
        {
            get; set;
        }

        string StoragePath
        {
            get; set;
        }

        bool IsNeedDecode { get; set; }

        bool DeleteTempFiles { get; set; }

        bool IsDownloadSpecial { get; set; }

        int DownloadTag { get; set; }

        bool IsCancelPending { get; set; }
        bool IsDownloading { get; set; }

         CancellationTokenSource CancellationTokenSource { get; }
    }
}
