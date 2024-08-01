using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using UtilityTools.Core.Models;

namespace UtilityTools.Controls.Converters
{
    public class BackgroundConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TaskStage stage = (TaskStage)value;
            switch (stage)
            {
                case TaskStage.None:
                    return "";
                case TaskStage.Doing:
                    return "0";
                case TaskStage.Error:
                    return "-1";
                case TaskStage.Done:
                    return "+1";
                case TaskStage.Copy:
                    return "2";
                case TaskStage.Prepare:
                    return "3";
                case TaskStage.Prepared:
                    return "4";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
