using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Read;

namespace UtilityTools.Services.Read
{
    public class BookMarkService : IBookMarkService
    {
        private readonly IRepository<BookMark> _bookMarkRepository;
        public BookMarkService(
            IRepository<BookMark> bookMarkRepository
            )
        {
            _bookMarkRepository = bookMarkRepository;
        }


        public void Add(BookMark mark)
        {
            mark.CreatedTime = DateTime.Now;
            _bookMarkRepository.Insert(mark);
        }

        public IList<BookMark> GetList(Book book)
        {
            return _bookMarkRepository.Table.Where(p => p.Book == book).ToList();
        }

        public async Task<IList<BookMark>> GetListAsync(Book book)
        {
            return await _bookMarkRepository.Table.Where(p => p.Book == book).ToListAsync();
        }


        public void Delete(int id)
        {
            var model = _bookMarkRepository.Table.Where(p => p.Id == id).FirstOrDefault();
            if (model != null)
            {
                _bookMarkRepository.Delete(model);
            }
        }
    }
}
