using System.Collections.Generic;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IBookMarkService
    {
        void Add(BookMark mark);
        IList<BookMark> GetList(Book book);
        Task<IList<BookMark>> GetListAsync(Book book);
        void Delete(int id);
    }
}