using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IBookHistoryService
    {
        void Clear();
        BookHistory GetBookInfo(string name);
        void Save(BookHistory history);
        void Delete(string name);
    }
}