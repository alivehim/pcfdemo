using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class FlowProperties
    {
        public string startTime { get;set; }
        public string endTime { get;set; }

        public string status { get; set; }

        public  FLowTrigger trigger { get;set;}
    }

    public class FLowTrigger
    {
        public string name { get; set; }
        public FlowTriggerLink inputsLink { get; set; }
        public FlowTriggerLink outputsLink { get; set; }
    }


    public class FlowTriggerLink
    {
        public string uri { get; set; }
    }
}
