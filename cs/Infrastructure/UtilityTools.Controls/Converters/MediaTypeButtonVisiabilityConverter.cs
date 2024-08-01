using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using UniversalFramework.Core.Extensions;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;

namespace UtilityTools.Controls.Converters
{
    //https://stackoverflow.com/questions/5454726/can-a-wpf-converter-access-the-control-to-which-it-is-bound
    //https://social.technet.microsoft.com/wiki/contents/articles/12423.wpfhowto-pass-and-use-a-control-in-it-s-own-valueconverter-for-convertconvertback.aspx
    public class MediaTypeButtonVisiabilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var myControl = values[0] as FrameworkElement;

                var name = myControl.Name;
                var mediaType = MediaType.File;
                if (
                    values[1] is MediaType)
                {
                    mediaType = (MediaType)values[1];
                }

                var attribute = mediaType.GetAttribute<ButtionDisplayAttribute>();

                if (attribute != null && attribute.ButtonNames != null && attribute.ButtonNames.Contains(name))
                {
                    return true;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return false;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
