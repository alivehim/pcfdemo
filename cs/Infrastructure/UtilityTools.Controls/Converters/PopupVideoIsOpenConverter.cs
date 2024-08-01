using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Controls.Converters
{

    [ValueConversion(typeof(bool), typeof(bool))]
    public class PopupVideoIsOpenConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values[2] is MediaType mediaType)
            {
                if (mediaType != MediaType.Magnet)
                    return false;

                if (values[3]==null)
                {
                    return false;
                }
            }

            return values.Take(2).ToArray().Any(value => value is bool && (bool)value);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();

            //throw new NotSupportedException();

            return new object[] { false };
        }
    }
}
