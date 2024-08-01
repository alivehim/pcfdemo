using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class KeywordItemViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Keyword { get; set; }

        public int Star { get; set; }
    }
}
