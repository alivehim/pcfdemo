using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Data
{
    public class SearchHistoryService : ISearchHistoryService
    {
        private IRepository<SearchHistoryItem> historyRepository;

        public SearchHistoryService(IRepository<SearchHistoryItem> historyRepository)
        {
            this.historyRepository = historyRepository;
        }


        public bool Delete(string owner,string item)
        {
            var result = historyRepository.Table.FirstOrDefault(p => p.Owner == owner && p.Url==item);

            if (result!=null)
            {

                return Delete(result);
            }

            return false;
        }

        public bool Delete(SearchHistoryItem item)
        {
            historyRepository.Delete(item);
            return true;
        }

        public IList<SearchHistoryItem> GetList(string owner, int pageIndex, int pageSize)
        {
            var list = historyRepository.Table.Where(p => p.Owner == owner);

            if (list.Count() > 0)
            {
                return list.OrderByDescending(p => p.LatestUsedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            return null;
        }


        public IList<SearchHistoryItem> GetListByDomain(string domain, int pageIndex, int pageSize)
        {

            if (!string.IsNullOrEmpty(domain))
            {

                var list = historyRepository.Table.Where(p => p.Url.StartsWith(domain));
                if (list.Count() > 0)
                {
                    return list.OrderByDescending(p => p.LatestUsedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            else
            {
                var list = historyRepository.Table.Where(p => !p.Url.StartsWith("http"));
                if (list.Count() > 0)
                {
                    return list.OrderByDescending(p => p.LatestUsedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }



            return null;
        }

        public IList<SearchHistoryItem> PageList(string owner, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public long Save(SearchHistoryItem item)
        {

            string url = item.Url;
            SearchHistoryItem db;
            if ((db = historyRepository.Table.FirstOrDefault(p => p.Url == url && p.Owner == item.Owner)) == null)
            {
                item.LatestUsedTime = DateTime.Now;
                historyRepository.Insert(item);
                return item.Id;
            }
            else
            {
                //更新访问时间
                db.LatestUsedTime = DateTime.Now;
                historyRepository.Update(db);
                return item.Id;
            }
        }
    }
}
