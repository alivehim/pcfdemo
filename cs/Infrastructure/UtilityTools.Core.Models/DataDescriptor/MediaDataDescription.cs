using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UtilityTools.Core.Models.MessageBus;

namespace UtilityTools.Core.Models.DataDescriptor
{
    public class MediaDataDescription : BaseResourceMetadata
    {
        public string Duration { get; set; }

        public string ExtensionName { get; set; }

        public string ImageUrl { get; set; }

        public string Url { get; set; }

        public string StoragePath { get; set; }

        public bool IsHD { get; set; }

        public int RawDuration { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public bool DeleteTempFiles { get; set; } = false;

        public MediaSymbolType MediaType { get; set; }

        public long RawSize { get; set; }

        public string Size { get; set; }

        public int Order { get; set; }

        public string PreviewUrl { get; set; }

        public byte[] ImageSource { get; set; }

        //public FileOrder Order { get; set; } = FileOrder.ByTimeDesc;
    }
}
