using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Core.Could
{
    public interface IMangaDownloader
    {
        Task<string> RunAsync(IMessage message,string name, string url);
    }
}
