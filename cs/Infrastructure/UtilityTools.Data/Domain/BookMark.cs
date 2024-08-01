using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class BookMark : BaseEntity
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Text { get; set; }

        public DateTime CreatedTime { get; set; }

        public double Position { get; set; }

        public int PageIndex { get; set; }

        public virtual Book Book { set; get; }
    }
}
