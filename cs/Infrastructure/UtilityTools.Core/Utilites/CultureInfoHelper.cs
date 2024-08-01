using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Utilites
{
    public class CultureInfoHelper
    {
        private static CultureInfo currentCulture = GetCurrentCulture();
        public static CultureInfo CurrentCulture => currentCulture;

        private static CultureInfo GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture;
        }
    }
}
