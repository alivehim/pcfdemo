using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class MediaKeyword : BaseEntity
    {
        public string Keyword { get; set; }

        public int Type { get; set; }

        public int? Star { get; set; }
    }
}
