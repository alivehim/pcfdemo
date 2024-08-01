using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Read
{
    public class BookInfo
    {
        public BookInfo()
        {
            this.Chapters = new List<Chapter>();
        }

        public string Name { get; set; }

        public string BookContent { get; set; }
        public List<Chapter> Chapters { get; set; }
        public int Current { get; set; }

        public string Path { get; set; }

    }
}
