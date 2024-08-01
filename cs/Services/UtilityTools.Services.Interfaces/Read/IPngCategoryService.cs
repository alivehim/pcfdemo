using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IPngCategoryService
    {
        PngCategory Add(string name);
        IList<PngCategory> GetList(bool refresh = false);
        Task<IList<PngCategory>> GetListAsync(bool refresh = false);
        PngCategory FindByName(string name);
        PngCategory FindByNameIncludeImages(string name);
        void Delete(string name);
    }
}
