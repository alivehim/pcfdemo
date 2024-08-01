using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models
{
    public class MagnetDescription
    {
        public string FileName { get; set; }
        public string Address { get; set; }
        public int Count { get; set; }

        public long RawSize { get; set; }

        public string Size { get; set; }

        public string Date { get; set; }
    }
}
