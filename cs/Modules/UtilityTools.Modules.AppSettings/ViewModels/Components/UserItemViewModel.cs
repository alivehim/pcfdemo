using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class UserItemViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
