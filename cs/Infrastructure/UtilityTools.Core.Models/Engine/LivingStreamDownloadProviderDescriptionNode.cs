using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.Magnet;

namespace UtilityTools.Core.Models.Engine
{
    public class LivingStreamDownloadProviderDescriptionNode
    {
        public LivingStreamDownloadProviderDescriptionNode(LivingStreamDownloadProvider  downloadProvider, Type handler)
        {
            DownloadProvider = downloadProvider;
            Handler = handler;
        }

        public LivingStreamDownloadProvider DownloadProvider { get; set; }
        public Type Handler { get; set; }
    }
}
