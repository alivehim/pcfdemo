using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class PngImage : BaseEntity
    {
        public PngImage()
        {
            PngImageLables = new HashSet<PngImageCategoryRelation>();
        }
        public string Location { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PngImageCategoryRelation> PngImageLables { set; get; }
    }
}
