using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Read
{
    public class Chapter
    {
        public Chapter()
        {
            this.Pages = new List<PageInfo>();
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Context { get; set; }
        /// <summary>
        /// 章节号
        /// </summary>
        public int ChapterNo { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        //public int Size { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        //public int PageNum { get; set; }

        public int PageCount { get { return Pages.Count; } }
        //public List<string> PageList { get; set; }
        public List<PageInfo> Pages { get; set; }

        public override string ToString()
        {
            return Title;
        }

        public int StartPageIndex { get; set; }

        public int EndPageIndex => StartPageIndex + PageCount;
    }
}
