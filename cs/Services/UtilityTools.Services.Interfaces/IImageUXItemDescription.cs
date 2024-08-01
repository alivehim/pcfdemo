using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces
{
    public interface IImageUXItemDescription : IBaseUXItemDescription
    {
        string Name { get; set; }
        string StoragePath
        {
            get; set;
        }
        int NumberOfChunkFiles { get; set; }

        double ChunkProgressPosition { get; set; }
        int ChunkDownloadPosition { get; set; }
        double ChunkCombinePosition { get; set; }
        int CompletedNumber { get; set; }
        int Progress
        {
            get;
            set;
        }
        string Url { get; set; }
        bool IsCancelPending { get; set; }
        bool IsDownloading { get; set; }
        //IList<MediaDataDescription> Items {get;set;}
    }
}
