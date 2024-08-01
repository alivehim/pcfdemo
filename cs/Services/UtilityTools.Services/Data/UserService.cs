using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Data
{
    public class UserService : IUserService
    {
        private IRepository<ResourceUser> repository;

        public UserService(IRepository<ResourceUser> repository)
        {
            this.repository = repository;
        }

        public void Add(ResourceUser resourceUser)
        {
            repository.Insert(resourceUser);
        }

        public void DeleteByName(string name)
        {
            var item = repository.Table.FirstOrDefault(p => p.Name == name);
            if (item != null)
            {
                repository.Delete(item);
            }
        }

        public IList<ResourceUser> GetAll()
        {
            return repository.Table.ToList();
        }
    }
}
