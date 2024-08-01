/* ErrorList
 * http://Suplanus.de by Johann Weiher
 * 
 * Control based on the Idea: http://errorlist.codeplex.com/
 * https://github.com/Suplanus/ErrorList.git
 */
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using UtilityTools.Core.Models.UX.ErrorList;

namespace UtilityTools.Controls.Panels
{
    public partial class ErrorListControl : IErrorList
    {
        private readonly ErrorListDataModel _dataContext = new ErrorListDataModel();

        protected static PropertyChangedCallback ItemsPropertyChangedCallback = new PropertyChangedCallback(ItemsPropertyChanged);

        public static DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached("Items", typeof(INotifyCollectionChanged), typeof(ErrorListControl), new PropertyMetadata(null, ItemsPropertyChangedCallback));

        private static void ItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ErrorListControl thisGrid = (ErrorListControl)sender;
            if (thisGrid == null)
            {
                return;
            }
            thisGrid.UnregisterItems(e.OldValue as INotifyCollectionChanged);
            thisGrid.RegisterItems(e.NewValue as INotifyCollectionChanged);
            thisGrid.Refresh();
        }

        public INotifyCollectionChanged Items
        {
            get
            {
                return (INotifyCollectionChanged)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        protected void UnregisterItems(INotifyCollectionChanged items)
        {
            if (items == null)
            {
                return;
            }
            items.CollectionChanged -= ItemsChanged;
        }

        protected void RegisterItems(INotifyCollectionChanged items)
        {
            if (items == null)
            {
                return;
            }

            var binds = items as ObservableCollection<ErrorListDataEntry>;
            ((INotifyCollectionChanged)items).CollectionChanged += MainWindow_CollectionChanged;
            DataBindingContext = binds;
            items.CollectionChanged += ItemsChanged;
        }

        protected virtual void UpdateValues()
        {
            System.Diagnostics.Debug.WriteLine("Updating values");
        }

        protected virtual void UpdateGrid()
        {
            System.Diagnostics.Debug.WriteLine("Updating grid");
        }

        public void Refresh()
        {
            UpdateValues();
            UpdateGrid();
        }

        protected virtual void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }
        public ErrorListControl()
        {
            InitializeComponent();

            dgv.DataContext = _dataContext;
            SetTextBoxBindings();

        }

        #region IErrorListReporter

        public ObservableCollection<ErrorListDataEntry> DataBindingContext
        {
            get { return _dataContext.ErrorListData; }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException("Unable to bind to a null reference");
                }

                _dataContext.ErrorListData = value;
            }
        }

        public bool ErrorsVisible
        {
            get
            {
                return tglBtnErrors.IsChecked.HasValue && tglBtnErrors.IsChecked.Value;
            }
            set
            {
                tglBtnErrors.IsChecked = value;
            }
        }

        public bool WarningsVisible
        {
            get
            {
                return tglBtnWarnings.IsChecked.HasValue && tglBtnWarnings.IsChecked.Value;
            }
            set
            {
                tglBtnWarnings.IsChecked = value;
            }
        }

        public bool MessagesVisible
        {
            get
            {
                return tglBtnMessages.IsChecked.HasValue && tglBtnMessages.IsChecked.Value;
            }
            set
            {
                tglBtnMessages.IsChecked = value;
            }
        }

        public void ClearAll()
        {
            _dataContext.ErrorListData = new ObservableCollection<ErrorListDataEntry>();
        }

        public void AddError(string description)
        {
            _dataContext.AddError(description);
        }

        public void AddWarning(string description)
        {
            _dataContext.AddWarning(description);
        }

        public void AddInformation(string description)
        {
            _dataContext.AddInformation(description);
        }

        #endregion IErrorListReporter

        #region EventHandlers

        private void Errors_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton tgl = (ToggleButton)sender;
            _dataContext.ShowErrors = tgl.IsChecked.HasValue && tgl.IsChecked.Value;
        }

        private void Errors_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton tgl = (ToggleButton)sender;
            _dataContext.ShowErrors = tgl.IsChecked.HasValue && tgl.IsChecked.Value;
        }

        private void Warnings_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton tgl = (ToggleButton)sender;
            _dataContext.ShowWarnings = tgl.IsChecked.HasValue && tgl.IsChecked.Value;
        }

        private void Warnings_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton tgl = (ToggleButton)sender;
            _dataContext.ShowWarnings = tgl.IsChecked.HasValue && tgl.IsChecked.Value;
        }

        private void Informations_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton tgl = (ToggleButton)sender;
            _dataContext.ShowInformations = tgl.IsChecked.HasValue && tgl.IsChecked.Value;
        }

        private void Informations_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton tgl = (ToggleButton)sender;
            _dataContext.ShowInformations = tgl.IsChecked.HasValue && tgl.IsChecked.Value;
        }

        #endregion EventHandlers

        private void SetTextBoxBindings()
        {
            txtErrors.DataContext = _dataContext;
            txtWarnings.DataContext = _dataContext;
            txtMessages.DataContext = _dataContext;
        }

        private void txtMessages_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var text = (sender as TextBox).Text;

            Clipboard.SetDataObject(text);
        }

        private void MainWindow_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            ////https://stackoverflow.com/questions/1027051/how-to-autoscroll-on-wpf-datagrid

            dgv.ScrollIntoView(CollectionView.NewItemPlaceholder);

        }
    }
}
