using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DB;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Infrastructure.MediaGet
{
    public interface IMediaSymbolService
    {
        MediaSymbolType GetSymbol(string url);
        IList<MediaSymboDescription> GetAllSymbols();
        string GetStoragePath(MediaSymbolType symbol);
    }
}
