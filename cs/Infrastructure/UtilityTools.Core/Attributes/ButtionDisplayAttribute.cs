using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Attributes
{
    public class ButtionDisplayAttribute : Attribute
    {
        public ButtionDisplayAttribute(params string[] buttonNames)
        {
            ButtonNames = buttonNames;
        }

        public string [] ButtonNames { get; set; }

    }
}
