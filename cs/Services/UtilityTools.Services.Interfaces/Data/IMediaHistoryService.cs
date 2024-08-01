using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Data
{
    public interface IMediaHistoryService
    {
        bool FindByName(string name,out MediaHistory data);

        void AddSearchHistory(string name,string path);

        bool FindByFuzzyName(string name,out MediaHistory data);

        IList<MediaHistory> GetAll();
    }
}
