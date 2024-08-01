using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Data
{
    public class MediaHistoryService : IMediaHistoryService
    {
        private readonly IRepository<MediaHistory> repository;

        public MediaHistoryService(IRepository<MediaHistory> repository)
        {
            this.repository = repository;
        }

        public IList<MediaHistory> GetAll()
        {
            var data = repository.Table.OrderByDescending(p => p.CreatedOn).Take(100).ToList();

            return data;
        }

        public void AddSearchHistory(string name, string path)
        {
            var data = repository.Table.Where(p => p.Name == name).ToList();

            if (data.Count > 0)
            {
                var firstData = data.First();

                firstData.UpdatedOn = DateTime.Now;

                repository.Update(firstData);
            }
            else
            {
                repository.Insert(new MediaHistory
                {
                    Name = name,
                    Action = ActionType.Search,
                    CreatedOn = DateTime.Now,
                    Path = path,
                    UpdatedOn = DateTime.Now
                });
            }


        }

        public bool FindByName(string name, out MediaHistory data)
        {
            data = repository.Table.FirstOrDefault(p => p.Name == name);
            return data != null;
        }

        public bool FindByFuzzyName(string name, out MediaHistory data)
        {
            data = repository.Table.FirstOrDefault(p => p.Name.Contains(name));
            return data != null;
        }

    }
}
