using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UtilityTools.Core.Models.UX
{
    public class SearchDropdownItem
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Address { get; set; }

        public BitmapImage Icon { get; set; }

        public string ShortIconName { get; set; }

        public DateTime ModifyTime { get; set; }

        public MediaSymbolType SymbolType { get; set; }
    }
}
