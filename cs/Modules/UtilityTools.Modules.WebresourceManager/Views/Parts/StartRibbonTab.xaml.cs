using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UtilityTools.Core.Models.UX;
using UtilityTools.Modules.WebresourceManager.ViewModels;
using static UtilityTools.Controls.AutoComplete;

namespace UtilityTools.Modules.WebresourceManager.Views.Parts
{
    /// <summary>
    /// Interaction logic for StartRibbonTab.xaml
    /// </summary>
    public partial class StartRibbonTab
    {
        public StartRibbonTab()
        {
            InitializeComponent();
        }

        private void RibbonTab_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected void auto_PatternChanged(object sender, AutoCompleteArgs args)
        {
            //check
            if (string.IsNullOrEmpty(args.Pattern))
                args.CancelBinding = true;
            else
                args.DataSource = GetSolutions(args.Pattern);
        }

        /// <summary>
        /// Get a list of cities that follow a pattern
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<SearchDropdownItem> GetSolutions(string Pattern)
        {
            var vm = DataContext as WebResourceManagerViewModel;
            if (!string.IsNullOrEmpty(Pattern))
            {
                // match on contain (could do starts with)
                return new ObservableCollection<SearchDropdownItem>(
                    vm.SolutionItems.
                    Where((city, match) => city.Name.ToLower().Contains(Pattern.ToLower())));

            }
            else
            {
                return vm.SolutionItems;
            }
        }
    }
}
