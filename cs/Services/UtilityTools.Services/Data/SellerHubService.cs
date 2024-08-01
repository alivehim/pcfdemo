using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Data
{
    public class SellerHubService : ISellerHubService
    {
        private IRepository<SellerHub> repository;

        public SellerHubService(IRepository<SellerHub> repository)
        {
            this.repository = repository;
        }

        public void Add(SellerHub sellerHub)
        {
            repository.Insert(sellerHub);
        }

        public void Update(SellerHub sellerHub)
        {
            var item = repository.Table.FirstOrDefault(p => p.SellerName == sellerHub.SellerName);
            if (item != null)
            {
                item.Address = sellerHub.Address;
                item.StoragePath = sellerHub.StoragePath;
                repository.Update(item);
            }
        }

        public void DeleteByName(string name)
        {
            var item = repository.Table.FirstOrDefault(p => p.SellerName == name);
            if (item != null)
            {
                repository.Delete(item);
            }
        }

        public IList<SellerHub> GetAll()
        {
            return repository.Table.ToList();
        }

        public SellerHub GetSeller(string name)
        {
            var item = repository.Table.FirstOrDefault(p => p.SellerName == name);
            return item;
        }

    }
}
