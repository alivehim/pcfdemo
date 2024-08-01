using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class BookCategoryRelation : BaseEntity
    {
        public int CategoryId { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public virtual BookCategory BookCategory { get; set; }

    }
}
