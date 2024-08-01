using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Engine
{
    public class StreamDownloadProviderDescriptionNode
    {
        public StreamDownloadProviderDescriptionNode(StreamDownloadProvider downloadProvider, Type handler)
        {
            DownloadProvider = downloadProvider;
            Handler = handler;
        }

        public StreamDownloadProvider DownloadProvider { get; set; }
        public Type Handler { get; set; }
    }
}
