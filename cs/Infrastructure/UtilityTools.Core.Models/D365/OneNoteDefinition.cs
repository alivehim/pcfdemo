using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class OneNoteDefinition
    {
        
    }

    public class BookResponse
    {
        public IList<BookItem> value { get; set; }
    }

    public class BookItem
    {
        public string id { get; set; }
        public string sectionsUrl { get; set; }

        public string displayName { get; set; }

        public string self { get; set; }
    }

    public class SectionResponse
    {
        public IList<SectionItem> value { get; set; }
    }

    public class SectionItem
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string self { get; set; }
        public string pagesUrl { get; set; }
    }

    public class PageResponse
    {

        [JsonProperty(PropertyName = "@odata.nextLink")]
        public string nextLink { get; set; }
        public IList<PageItem> value { get; set; }
    }

    public class PageItem
    {
        public string id { get; set; }
        public string title { get; set; }

        public string contentUrl { get; set; }
    }

}
