using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.Navigator.ViewModels
{
    public class MenuItemDesription : ViewModelBase
    {
        private bool visiable = true;
        public string Name { get; set; }
        public string ImageSource { get; set; }
        public string ToolTips { get; set; }

        public bool Visiable
        {
            get
            {
                return visiable;
            }
            set
            {
                visiable = value;
                RaisePropertyChanged("Visiable");
            }
        }
    }
}
