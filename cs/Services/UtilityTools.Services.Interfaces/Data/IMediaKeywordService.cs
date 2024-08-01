using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Data
{
    public interface IMediaKeywordService
    {
        void Add(MediaKeyword mediaKeyword);

        void DeleteByName(string name,int type=0);

        IList<MediaKeyword> GetAll(int type = 0);

        void Update(MediaKeyword mediaKeyword);
    }
}
