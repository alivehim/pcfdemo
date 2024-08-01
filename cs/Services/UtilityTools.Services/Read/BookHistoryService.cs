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
    public class BookHistoryService : IBookHistoryService
    {
        private readonly IRepository<BookHistory> _bookHistoryRepository;
        public BookHistoryService(IRepository<BookHistory> bookHistoryRepository)
        {
            _bookHistoryRepository = bookHistoryRepository;
        }

        public void Clear()
        {
            foreach (var item in _bookHistoryRepository.Table)
            {
                _bookHistoryRepository.Delete(item);
            }
        }
        public BookHistory GetBookInfo(string name)
        {
            return _bookHistoryRepository.Table.FirstOrDefault(p => p.Book.Name == name);
        }

        public void Save(BookHistory history)
        {
            if (history.Id == 0)
            {
                _bookHistoryRepository.Insert(history);
            }
            else
            {
                _bookHistoryRepository.Update(history);
            }
        }

        public void Delete(string name)
        {
            var model = _bookHistoryRepository.Table.FirstOrDefault(p => p.Book.Name == name);
            if (model != null)
            {
                _bookHistoryRepository.Delete(model);
            }
        }

    }
}
