using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using UtilityTools.Core.Models;

namespace UtilityTools.Controls.Converters
{
    public class StageBackgroundConverter: IValueConverter
    {

           //<DataTrigger Binding = "{Binding DownloadTaskStage, Converter={StaticResource bgConverter}}" Value="+1">
           //             <Setter Property = "Background" Value="{StaticResource ProfitBrush}"/>
           //         </DataTrigger>

           //         <!-- When a customer is owed money, color them red. -->
           //         <DataTrigger Binding = "{Binding DownloadTaskStage, Converter={StaticResource bgConverter}}" Value= "-1" >
           //             < Setter Property= "Background" Value= "{StaticResource LossBrush}" />
           //         </ DataTrigger >

           //         < DataTrigger Binding= "{Binding DownloadTaskStage, Converter={StaticResource bgConverter}}" Value= "0" >
           //             < Setter Property= "Background" Value= "{StaticResource YellowBrush}" />
           //         </ DataTrigger >
           //         < DataTrigger Binding= "{Binding DownloadTaskStage, Converter={StaticResource bgConverter}}" Value= "2" >
           //             < Setter Property= "Background" Value= "{StaticResource OkBrush}" />
           //         </ DataTrigger >

           //         < DataTrigger Binding= "{Binding DownloadTaskStage, Converter={StaticResource bgConverter}}" Value= "3" >
           //             < Setter Property= "Background" Value= "{StaticResource PrepareBrush}" />
           //         </ DataTrigger >

           //         < DataTrigger Binding= "{Binding DownloadTaskStage, Converter={StaticResource bgConverter}}" Value= "4" >
           //             < Setter Property= "Background" Value= "{StaticResource PreparedBrush}" />
           //         </ DataTrigger >

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TaskStage stage = (TaskStage)value;
            switch (stage)
            {
                case TaskStage.None:
                    //return System.Windows.Media.Brushes.Transparent;
                    return Application.Current.Resources["WindowBackgroundBrush"];
                case TaskStage.Doing:
                    return Application.Current.Resources["YellowBrush"];
                case TaskStage.Error:
                    return Application.Current.Resources["LossBrush"];
                case TaskStage.Done:
                    return Application.Current.Resources["ProfitBrush"];
                case TaskStage.Copy:
                    return Application.Current.Resources["OkBrush"];
                case TaskStage.Prepare:
                    return Application.Current.Resources["PrepareBrush"]; 
                case TaskStage.Prepared:
                    return Application.Current.Resources["PreparedBrush"];
                case TaskStage.Viewed:
                    return Application.Current.Resources["ViewedBrush"];
            }
            return new SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
