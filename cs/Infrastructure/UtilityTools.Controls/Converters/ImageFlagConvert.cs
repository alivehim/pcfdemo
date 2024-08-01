using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace UtilityTools.Controls.Converters
{
    public class ImageFlagConvert: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isUsed = (bool)value;

            if(isUsed)
            {
                return new BitmapImage(new Uri("pack://application:,,,/DownloadApp.Common;component/Resources/Images/flag.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                return new BitmapImage(new Uri("pack://application:,,,/DownloadApp.Common;component/Resources/Images/page_blank.png", UriKind.RelativeOrAbsolute));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
