using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.AutomateFlow.ViewModels
{
    public class FlowUXItemDescription : BaseUXItemDescription
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ObjectId { get; set; }

        public string WorkflowId { get; set; }
    }
}
