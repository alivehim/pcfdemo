using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtilityTools.Modules.WebresourceManager.ViewModels;

namespace UtilityTools.Modules.WebresourceManager.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WebResourceManagerView : UserControl
    {
        public WebResourceManagerView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var db = this.DataContext as WebResourceManagerViewModel;
            Observable.FromEventPattern<TextChangedEventArgs>(this.fileFilter, "TextChanged").Throttle(TimeSpan.FromSeconds(1)).Subscribe(x =>
                this.Dispatcher.Invoke(() =>
                {
                    var filterText = this.fileFilter.Text;
                    db.NameFilter = filterText;
                }));

        }
    }
}