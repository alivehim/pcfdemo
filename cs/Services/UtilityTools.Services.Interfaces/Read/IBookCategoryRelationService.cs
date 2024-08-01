using System.Collections.Generic;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IBookCategoryRelationService
    {
        void Clear();
        void Add(BookCategoryRelation model);
        void Delete(int bookId, int categoryId);
        BookCategoryRelation FindById(int bookId, int categoryId);
    }
}