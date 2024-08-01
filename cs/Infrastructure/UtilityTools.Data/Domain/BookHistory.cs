using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class BookHistory : BaseEntity
    {
        
        public int PageIndex
        {
            get; set;
        }

        public double Position { get; set; }

        public int Length { get; set; }
        public DateTime LastReadTime { get; set; }

        public virtual Book Book { set; get; }


    }
}
