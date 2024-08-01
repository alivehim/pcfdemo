using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.Core.Models;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Data
{
    public class MediaSymbolDBService : IMediaSymbolDBService
    {
        private IRepository<MediaSymbol> repository;

        public MediaSymbolDBService(IRepository<MediaSymbol> repository)
        {
            this.repository = repository;
        }

        public void Add(MediaSymbol mediaSymbol)
        {
            repository.Insert(mediaSymbol);
        }

        public void Update(MediaSymbol mediaSymbol)
        {
            var item = repository.Table.FirstOrDefault(p => p.Symbol == mediaSymbol.Symbol);
            if (item != null)
            {
                item.Address= mediaSymbol.Address;
                item.StoragePath= mediaSymbol.StoragePath;
                repository.Update(item);
            }
        }

        public void DeleteByName(string name)
        {
            var item = repository.Table.FirstOrDefault(p => p.Symbol == name);
            if (item != null)
            {
                repository.Delete(item);
            }
        }

        public IList<MediaSymbol> GetAll()
        {
            return repository.Table.ToList();
        }

        public MediaSymbol GetMediaSymbol(string symbol)
        {
            var item = repository.Table.FirstOrDefault(p => p.Symbol == symbol);
            return item;   
        }  
        
        public MediaSymbol GetMediaSymbol(MediaSymbolType symbol)
        {
            var item = repository.Table.FirstOrDefault(p => p.Symbol == symbol.ToString());
            return item;   
        }
    }
}
