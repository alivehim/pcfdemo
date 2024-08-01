using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class MediaHistory : BaseEntity
    {
        public string Name { get; set; }

        public string Path { get; set; }
        public ActionType Action { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set;}
    }
}
