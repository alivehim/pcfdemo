using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Data
{
    public interface IMediaSymbolDBService
    {
        void Add(MediaSymbol mediaSymbol);

        void Update(MediaSymbol mediaSymbol);
        void DeleteByName(string name);

        IList<MediaSymbol> GetAll();

        MediaSymbol GetMediaSymbol(string symbol);
        MediaSymbol GetMediaSymbol(MediaSymbolType symbol);
    }
}
