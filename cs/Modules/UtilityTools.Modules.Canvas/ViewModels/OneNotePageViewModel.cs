using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Modules.Canvas.ViewModels
{
    public class OneNotePageViewModel : BindableBase
    {
        public string Name { get; set; }

        public string ContentUrl { get; set; }
    }
}
