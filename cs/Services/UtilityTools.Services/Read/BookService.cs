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
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Book Add(Book book)
        {
            var model = _bookRepository.Table.FirstOrDefault(p => p.Name == book.Name && p.FullPath == book.FullPath);
            if (model != null)
            {
                return model;
            }
            else
            {
                _bookRepository.Insert(book);
                return book;
            }
        }

        public void Update(Book book)
        {
            _bookRepository.Update(book);
        }

        public Book Load(string name, string fullName)
        {
            var model = _bookRepository.Table
                .Include(p=>p.BookHistories)
                .Include(p=>p.BookMarks)
                .FirstOrDefault(p => p.Name == name && p.FullPath == fullName);
            if (model != null)
            {
                return model;
            }
            else
            {
                var newmodel = new Book { Name = name, FullPath = fullName };
                _bookRepository.Insert(newmodel);
                return newmodel;
            }
        }

        public void Delete(Book book)
        {
            _bookRepository.Delete(book);
        }

        public void Delete(string bookname)
        {
            var Model = _bookRepository.Table.FirstOrDefault(p => p.Name == bookname);

            if (Model != null)
            {
                _bookRepository.Delete(Model);
            }
        }

        public Book FindByName(string name)
        {
            return _bookRepository.Table.FirstOrDefault(p => p.Name == name);
        }


        public Book FindByNameEx(string name)
        {
            return _bookRepository.Table.Include(p => p.BookCategoryRelations).FirstOrDefault(p => p.Name == name);
        }
    }
}
