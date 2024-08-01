using System.Collections.Generic;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IBookCategoryService
    {
        void Clear();
        BookCategory Add(string name);
        void Delete(string name);
        Task<IList<BookCategory>> GetAll();
    }
}