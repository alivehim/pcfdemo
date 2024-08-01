using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Read
{
    public class NovelReadedHistory
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime ReadedTime { get; set; }
    }
}
