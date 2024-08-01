using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class FlowHistoryCollection
    {
        public IList<FlowHistory> value { get; set; }
        public string nextLink { get; set; }
    }
    public class FlowHistory
    {
        public string name { get; set; }
        public string id { get; set; }

        public FlowProperties properties { get; set; }
    }

    public class TriggerDetail
    {
        public ExpandoObject body { get; set; }
    }
}
