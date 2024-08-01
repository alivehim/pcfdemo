using System.Reactive.Linq;
using System;
using System.Windows.Controls;
using UtilityTools.Modules.AppSettings.ViewModels;

namespace UtilityTools.Modules.AppSettings.Views
{
    /// <summary>
    /// Interaction logic for MediaHubConfigrationView
    /// </summary>
    public partial class MediaHubConfigrationView : UserControl
    {
        public MediaHubConfigrationView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var db = this.DataContext as MediaHubConfigrationViewModel;
            Observable.FromEventPattern<TextChangedEventArgs>(this.fileFilter, "TextChanged").Throttle(TimeSpan.FromSeconds(1)).Subscribe(x =>
                this.Dispatcher.Invoke(() =>
                {
                    var filterText = this.fileFilter.Text;
                    db.NameFilter = filterText;
                }));
        }
    }
}
