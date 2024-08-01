using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using UtilityTools.Core.Models;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Controls.Converters
{

    //https://stackoverflow.com/questions/46671458/wpf-net-popup-open-on-hover-and-keep-open-if-mouse-is-over
    [ValueConversion(typeof(bool), typeof(bool))]
    public class PopupIsOpenConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values[2] is MediaType mediaType)
            {
                if (mediaType != MediaType.JAVDBMagnet && mediaType != MediaType.CloudStreamMeida)
                    return false;

                if (values[3] is IEnumerable<ViewModelBase> list)
                {
                    if(list.Count()==0)
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
