using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class TimeLineDefinition
    {
        public string aia_wf_requesttimelineid { get; set; }

        public string aia_actionon { get; set; }
}

    public class TimeLineCollection
    {
        public IList<TimeLineDefinition> value { get; set; }
    }
}
