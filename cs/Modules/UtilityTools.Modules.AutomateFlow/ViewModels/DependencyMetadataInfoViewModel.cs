using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.AutomateFlow.ViewModels
{
    public class DependencyMetadataInfoViewModel: ViewModelBase
    {
        public string Name { get; set; }

        public string Type { get; set; }
        public string Id { get; set; }
    }
}
