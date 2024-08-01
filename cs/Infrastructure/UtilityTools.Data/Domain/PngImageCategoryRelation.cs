using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class PngImageCategoryRelation: BaseEntity
    {
        public new int Id { get; set; }
        public int PngImageId { get; set; }
        public int LabelId { get; set; }

        public virtual PngImage PngImage { get; set; }
        public virtual PngCategory PngCategory { get; set; }
    }
}
