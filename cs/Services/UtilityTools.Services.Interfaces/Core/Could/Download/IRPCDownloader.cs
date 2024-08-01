using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Core.Could.Download
{
    public interface IRPCDownloader
    {
        string Address { get; }
        bool DownloadFile(string urlString, string fileName);
    }
}
