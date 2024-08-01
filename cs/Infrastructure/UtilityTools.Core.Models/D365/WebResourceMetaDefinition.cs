using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class WebResourceMetadata
    {
        public string msdyn_name { get; set; }
        public string msdyn_objectid { get; set; }
        public string msdyn_displayname { get; set; }

        public string msdyn_schemaname { get; set; }
    }

    public class WebResourceMetaDefinition
    {
        public IList<WebResourceMetadata> value { get; set; }
    }

}
