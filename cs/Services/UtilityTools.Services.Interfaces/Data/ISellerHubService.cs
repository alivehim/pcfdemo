using System.Collections.Generic;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Data
{
    public interface ISellerHubService
    {
        void Add(SellerHub sellerHub);
        void DeleteByName(string name);
        IList<SellerHub> GetAll();
        SellerHub GetSeller(string name);
        void Update(SellerHub sellerHub);
    }
}