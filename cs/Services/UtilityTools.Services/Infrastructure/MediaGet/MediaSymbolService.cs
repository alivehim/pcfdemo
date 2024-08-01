using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DB;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;

namespace UtilityTools.Services.Infrastructure.MediaGet
{
    public class MediaSymbolService : IMediaSymbolService
    {
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public MediaSymbolService(IMediaSymbolDBService mediaSymbolDBService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        /// <summary>
        /// get symbol by url 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MediaSymbolType GetSymbol(string url)
        {
            var list = mediaSymbolDBService.GetAll();
            if (!list.IsNullOrEmpty())
            {
                var result = list.FirstOrDefault(p => url.StartsWith(p.Address));
                if (result != null)
                {
                    return Enum.Parse<MediaSymbolType>(result.Symbol);
                }
            }
            return MediaSymbolType.None;
        }

        public IList<MediaSymboDescription> GetAllSymbols()
        {
            var result = new List<MediaSymboDescription>();

            var list = mediaSymbolDBService.GetAll();
            foreach (var item in list)
            {
                result.Add(new MediaSymboDescription
                {
                    SymbolType = Enum.Parse<MediaSymbolType>(item.Symbol),
                    Address = item.Address,
                    Name = item.Symbol.ToString()
                });
            }
            return result;
        }

        public string GetStoragePath(MediaSymbolType symbol)
        {
            var list = mediaSymbolDBService.GetAll();
            if (!list.IsNullOrEmpty())
            {
                var result = list.FirstOrDefault(p => p.Symbol == symbol.ToString());
                if (result != null)
                {
                    return result.StoragePath;
                }
            }
            return string.Empty;
        }
    }
}
