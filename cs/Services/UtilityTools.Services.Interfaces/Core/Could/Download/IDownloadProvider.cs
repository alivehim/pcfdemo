using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Core.Could.Download
{
    public interface IDownloadProvider
    {
        IRPCDownloader[] NormalDownloads { get; }
        IRPCDownloader[] SpecialDownloads { get; }
    }
}
