using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UtilityTools.Core.Infrastructure;

namespace UtilityTools.Controls.Converters
{
    public class ImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pngName = parameter as string;
            string[] parameters = pngName.Split(new char[] { '|' });

            if (parameters.Length == 1)
            {
                return ImageCachePool.GetCustomImageSource(parameters[0]);
            }
            else
            {
                return ImageCachePool.GetDefaultImageSource(parameters[0], parameters[1]);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
