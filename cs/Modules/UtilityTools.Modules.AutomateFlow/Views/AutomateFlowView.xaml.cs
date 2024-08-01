using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using UtilityTools.Modules.AutomateFlow.ViewModels;

namespace UtilityTools.Modules.AutomateFlow.Views
{
    /// <summary>
    /// Interaction logic for AutomateFlowView
    /// </summary>
    public partial class AutomateFlowView : UserControl
    {
        public AutomateFlowView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var db = this.DataContext as AutomateFlowViewModel;
            Observable.FromEventPattern<TextChangedEventArgs>(this.fileFilter, "TextChanged").Throttle(TimeSpan.FromSeconds(1)).Subscribe(x =>
                this.Dispatcher.Invoke(() =>
                {
                    var filterText = this.fileFilter.Text;
                    db.NameFilter = filterText;
                }));

        }

    }
}
