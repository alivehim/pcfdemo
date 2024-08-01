using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.DataManager.ViewModels
{
    public class RequestDescription : BaseUXItemDescription
    {
        public string Name { get; set; }

        public string FormId { get; set; }

        public string WFRequestId { get; set; }

        public string Priority { get; set; }

        public string LeadtimeReminder { get; set; }
    }
}
