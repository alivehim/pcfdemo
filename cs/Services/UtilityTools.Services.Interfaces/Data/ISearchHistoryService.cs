using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Data
{
    public interface ISearchHistoryService
    {
        IList<SearchHistoryItem> GetList(string owner, int pageIndex, int pageSize);
        long Save(SearchHistoryItem item);
        bool Delete(SearchHistoryItem item);
        IList<SearchHistoryItem> PageList(string owner, int pageIndex, int pageSize);

        IList<SearchHistoryItem> GetListByDomain(string domain, int pageIndex, int pageSize);
        bool Delete(string owner, string item);
    }
}
