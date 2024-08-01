using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Modules.Canvas.ViewModels
{
    internal class FlowHistoryViewModel
    {
        public string Name { get; set; }
        public string ID { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Status { get; set; }
    }
}
