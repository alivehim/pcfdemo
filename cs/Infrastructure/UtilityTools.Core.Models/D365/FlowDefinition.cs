using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class FlowDefinitionCollection
    {
        public IList<FlowDefinition> value { get; set; }  
    }

    public class FlowDefinition
    {
        public string msdyn_name { get; set; }
        public string msdyn_displayname { get; set; }
        public string msdyn_objectid { get; set; }
        public string msdyn_workflowidunique { get; set; }
    }
}
