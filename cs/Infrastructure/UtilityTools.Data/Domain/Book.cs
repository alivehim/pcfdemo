using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class Book : BaseEntity
    {
        public Book()
        {
            BookCategoryRelations = new HashSet<BookCategoryRelation>();
        }
        public string Name { get; set; }
        public string NovelName { get; set; }
        public string FullPath { get; set; }

        public int Type { get; set; }
        public virtual ICollection<BookCategoryRelation> BookCategoryRelations { set; get; }

        public virtual ICollection<BookHistory> BookHistories { set; get; }

        public virtual ICollection<BookMark> BookMarks { set; get; }
    }
}
