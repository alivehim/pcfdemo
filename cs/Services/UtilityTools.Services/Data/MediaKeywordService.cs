using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Data
{
    public class MediaKeywordService : IMediaKeywordService
    {
        private readonly IRepository<MediaKeyword> repository;

        public MediaKeywordService(IRepository<MediaKeyword> repository)
        {
            this.repository = repository;
        }

        public void Add(MediaKeyword mediaKeyword)
        {
            repository.Insert(mediaKeyword);
        }

        public void Update(MediaKeyword mediaKeyword)
        {
            var keyword = repository.Table.FirstOrDefault(p => p.Id==mediaKeyword.Id);
            keyword.Keyword = mediaKeyword.Keyword;

            keyword.Star = mediaKeyword.Star;
            repository.Update(keyword);
        }

        public void DeleteByName(string name, int type = 0)
        {
            var item = repository.Table.FirstOrDefault(p => p.Keyword == name && p.Type == type);
            if (item != null)
            {
                repository.Delete(item);
            }
        }

        public IList<MediaKeyword> GetAll(int type = 0)
        {
            return repository.Table.Where(p => p.Type == type).ToList();
        }
    }
}
