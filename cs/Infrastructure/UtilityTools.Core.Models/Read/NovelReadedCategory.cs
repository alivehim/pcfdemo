using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Read
{
    public class NovelReadedCategory
    {
        public string Title { get; set; }
        public IEnumerable<NovelReadedHistory> NovelReadedHistories { get; set; }
    }
}
