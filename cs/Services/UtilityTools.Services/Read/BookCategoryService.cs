using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Read;

namespace UtilityTools.Services.Read
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IRepository<BookCategory> _bookCategoryRepository;
        public BookCategoryService(IRepository<BookCategory> bookCategoryRepository)
        {
            _bookCategoryRepository = bookCategoryRepository;
        }

        public void Clear()
        {
            foreach (var item in _bookCategoryRepository.Table)
            {
                _bookCategoryRepository.Delete(item);
            }
        }

        public async Task<IList<BookCategory>> GetAll()
        {
            //return await _bookCategoryRepository.Table.ToListAsync();F
            return await _bookCategoryRepository.Table.Include(p => p.BookCategoryRelations).ThenInclude(img => img.Book).ToListAsync();
        }

        //public void Add(string name)
        //{
        //    if (_bookCategoryRepository.Table.FirstOrDefault(p => p.Name == name) != null)
        //        return;
        //    _bookCategoryRepository.Insert(new BookCategory {  Name = name });
        //}

        public BookCategory Add(string name)
        {
            var label = _bookCategoryRepository.Table.FirstOrDefault(p => p.Name == name);
            if (label != null)
                return label;
            else
            {
                var newlabel = new BookCategory { Name = name };
                _bookCategoryRepository.Insert(newlabel);
                return newlabel;
            }

        }

        public void Delete(string name)
        {
            var model = _bookCategoryRepository.Table.FirstOrDefault(p => p.Name == name);
            if (model != null)
            {
                _bookCategoryRepository.Delete(model);
            }
        }
    }
}
