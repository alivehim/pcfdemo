using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class MediaSymbolItemViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public string Path { get; set; }
    }
}
