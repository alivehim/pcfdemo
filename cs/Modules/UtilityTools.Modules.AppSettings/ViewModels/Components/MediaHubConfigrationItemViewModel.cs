using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class MediaHubConfigrationItemViewModel : ViewModelBase
    {
        public string Seller { get; set; }
        public string Address { get; set; }
        public string Path { get; set; }
    }
}
