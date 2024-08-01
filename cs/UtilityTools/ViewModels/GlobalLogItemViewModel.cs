using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.ViewModels
{

    public class GlobalLogItemViewModel : BindableBase
    {
        public string Message { get; set; }
        public ErrorListLevel Level { get; set; }
    }
}
