using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using UtilityTools.Core.Models;

namespace UtilityTools.Controls.Converters
{
    public class LoadedBehaviorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values[0] is MediaType)
            {
                var mediaType = (MediaType)values[0];
                string previewUrl = values[1] as string;

                return mediaType == MediaType.Magnet && !string.IsNullOrEmpty(previewUrl);
            }

            return false;
        
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
