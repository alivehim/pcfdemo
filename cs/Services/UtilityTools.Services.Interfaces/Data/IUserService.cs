using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Data
{
    public interface IUserService
    {
        void Add(ResourceUser resourceUser);

        void DeleteByName(string name);

        IList<ResourceUser> GetAll();
    }
}
