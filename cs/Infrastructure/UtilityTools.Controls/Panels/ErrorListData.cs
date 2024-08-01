using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.UX.ErrorList;

namespace UtilityTools.Controls.Panels
{


    public class ErrorListDataModel : INotifyPropertyChanged
    {

        public ErrorListDataModel()
        {
#if DEBUG
            // Performance-Test
            //for (int i = 0; i < 1000; i++)
            //{
            //this.AddError("Error unable to do something \"Name: Write PHP\", because the syntax is so looooooooooong");
            //this.AddError("Error unable to do something \"Name: Write Flash\"");
            //this.AddWarning("Error unable to do something \"Name: Program in F#, yet\"");
            //this.AddInformation("Note: I need a better hobby than wasting my lunch coding..");

            //this.AddError("Error unable to do something \"Name: Write PHP\", because the syntax is so looooooooooong");
            //this.AddError("Error unable to do something \"Name: Write Flash\"");
            //this.AddWarning("Error unable to do something \"Name: Program in F#, yet\"");
            //this.AddInformation("Note: I need a better hobby than wasting my lunch coding..");

            //this.AddError("Error unable to do something \"Name: Write PHP\", because the syntax is so looooooooooong");
            //this.AddError("Error unable to do something \"Name: Write Flash\"");
            //this.AddWarning("Error unable to do something \"Name: Program in F#, yet\"");
            //this.AddInformation("Note: I need a better hobby than wasting my lunch coding..");
            //}
#endif
        }

        public string ErrorsText
        {
            get
            {
                return string.Format("{0} Errors", _errorListData.Count(ed => ed.Level == ErrorListLevel.Error));
            }
        }

        public string WarningsText
        {
            get
            {
                return string.Format("{0} Warnings", _errorListData.Count(ed => ed.Level == ErrorListLevel.Warning));
            }
        }

        public string InformationsText
        {
            get
            {
                return string.Format("{0} Informations", _errorListData.Count(ed => ed.Level == ErrorListLevel.Information));
            }
        }

        public void AddError(string description)
        {
            _errorListData.Add(new ErrorListDataEntry { Description = description, Level = ErrorListLevel.Error });
            SetView();
        }

        public void AddWarning(string description)
        {
            _errorListData.Add(new ErrorListDataEntry { Description = description, Level = ErrorListLevel.Warning });
            SetView();
        }

        public void AddInformation(string description)
        {
            _errorListData.Add(new ErrorListDataEntry { Description = description, Level = ErrorListLevel.Information });
            SetView();
        }

        public ObservableCollection<ErrorListDataEntry> ErrorListData
        {
            get
            {
                return _errorListDataView;
            }
            internal set
            {
                _errorListData = value;
                _errorListData.CollectionChanged += MyItemsSource_CollectionChanged;
                SetView();
            }
        }

        private void MyItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                SetView();
            }
        }
        public bool ShowErrors
        {
            set
            {
                _showErrors = value;
                SetView();
            }
        }

        public bool ShowWarnings
        {
            set
            {
                _showWarnings = value;
                SetView();
            }
        }

        public bool ShowInformations
        {
            set
            {
                _showInformations = value;
                SetView();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void SetView()
        {
            var dispatcher = Application.Current.Dispatcher;
            Task.Run(() =>
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    var selectedLevels = new List<ErrorListLevel>();
                    if (_showErrors)
                        selectedLevels.Add(ErrorListLevel.Error);
                    if (_showWarnings)
                        selectedLevels.Add(ErrorListLevel.Warning);
                    if (_showInformations)
                        selectedLevels.Add(ErrorListLevel.Information);

                    _errorListDataView.Clear();
                    var selectedErrors = _errorListData.Where(ed => selectedLevels.Contains(ed.Level));
                    foreach (var selectedError in selectedErrors)
                        _errorListDataView.Add(selectedError);

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ErrorsText"));
                        PropertyChanged(this, new PropertyChangedEventArgs("WarningsText"));
                        PropertyChanged(this, new PropertyChangedEventArgs("InformationsText"));
                    }


                }));
            });


        }

        private ObservableCollection<ErrorListDataEntry> _errorListData = new ObservableCollection<ErrorListDataEntry>();
        private readonly ObservableCollection<ErrorListDataEntry> _errorListDataView = new ObservableCollection<ErrorListDataEntry>();

        private bool _showErrors = true;
        private bool _showWarnings = true;
        private bool _showInformations = true;


    }


}
