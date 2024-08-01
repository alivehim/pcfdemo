using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class BookCategory:BaseEntity
    {

        public BookCategory()
        {
            BookCategoryRelations = new HashSet<BookCategoryRelation>();
        }

        public string Name { get; set; }

        public virtual ICollection<BookCategoryRelation> BookCategoryRelations  { set; get; }

    }
}
