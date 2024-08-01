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
    public class BookCategoryRelationService : IBookCategoryRelationService
    {
        private readonly IRepository<BookCategoryRelation> _bookCategoryRelationRepository;
        private readonly IRepository<BookCategory> _bookCategoryRepository;
        private readonly IRepository<BookHistory> _bookHistoryRepository;
        public BookCategoryRelationService(IRepository<BookCategoryRelation> bookCategoryRelationRepository,
            IRepository<BookCategory> bookCategoryRepository,
             IRepository<BookHistory> bookHistoryRepository)
        {
            _bookCategoryRelationRepository = bookCategoryRelationRepository;
            _bookCategoryRepository = bookCategoryRepository;
            _bookHistoryRepository = bookHistoryRepository;
        }
        public void Clear()
        {
            foreach (var item in _bookCategoryRelationRepository.Table)
            {
                _bookCategoryRelationRepository.Delete(item);
            }
        }


        public void Add(BookCategoryRelation model)
        {

            var states = _bookCategoryRelationRepository.Attach<BookCategory>(model.BookCategory);
            if (model.Book.Id != 0)
            {
                var relation = _bookCategoryRelationRepository.Table.FirstOrDefault(p => p.BookId == model.Book.Id && p.CategoryId == model.BookCategory.Id);
                if (relation != null)
                    return;
                _bookCategoryRelationRepository.Attach<Book>(model.Book);
            }
            _bookCategoryRelationRepository.Insert(model);
        }

        public void Delete(int bookId, int categoryId)
        {
            var model = _bookCategoryRelationRepository.Table.FirstOrDefault(p => p.CategoryId == categoryId && p.BookId == bookId);
            if (model != null)
            {
                _bookCategoryRelationRepository.Delete(model);
            }
        }

        public BookCategoryRelation FindById(int bookId, int categoryId)
        {
            return _bookCategoryRelationRepository.Table.FirstOrDefault(p => p.CategoryId == categoryId && p.BookId == bookId);
        }



        //public void Clear(int historyId)
        //{
        //    foreach (var item in _bookCategoryRelationRepository.Table.Where(p => p.HistoryId == historyId))
        //    {
        //        _bookCategoryRelationRepository.Delete(item);
        //    }
        //}

        //public void Add(int historyId, int categoryId)
        //{
        //    _bookCategoryRelationRepository.Insert(new BookCategoryRelation
        //    {
        //        CategoryId = categoryId,
        //        HistoryId = historyId
        //    });
        //}

        //public IList<BookCategory> GetCategoryFromBook(int historyId)
        //{
        //    var items = _bookCategoryRelationRepository.Table.Where(p => p.HistoryId == historyId).Select(p => p.CategoryId).ToList();

        //    if (items != null && items.Count != 0)
        //    {
        //        return _bookCategoryRepository.Table.Where(p => items.Contains(p.Id)).ToList();
        //    }
        //    return null;
        //}

        //public IList<NovelReadedCategory> GetBookCategoryRelations()
        //{
        //    IList<NovelReadedCategory> list = new List<NovelReadedCategory>();

        //    var query = from y in (from m in _bookCategoryRelationRepository.Table
        //                           join x in _bookCategoryRepository.Table on m.CategoryId equals x.Id
        //                           join n in _bookHistoryRepository.Table on m.HistoryId equals n.Id

        //                           select new { x.Name, n.FileName, n.LastReadTime, BookName= n.Name })
        //                group y by y.Name into g
        //                select g;

        //    foreach (var item in query)
        //    {
        //        list.Add(new NovelReadedCategory
        //        {
        //            Title = item.Key,
        //            NovelReadedHistories = item.Select(p => new NovelReadedHistory
        //            {
        //                Path = p.FileName,
        //                ReadedTime = p.LastReadTime,
        //                Name = p.BookName
        //            })
        //        });
        //    }
        //    return list;
        //}
    }
}
