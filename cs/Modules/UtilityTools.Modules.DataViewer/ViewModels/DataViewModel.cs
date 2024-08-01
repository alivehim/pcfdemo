using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Core.Mvvm;
using UtilityTools.Modules.DataViewer.Core;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Modules.DataViewer.ViewModels
{
    [MessageOwner(MessageOwner.DataViewer)]
    public class DataViewModel : ViewModelBase
    {

        private readonly IMediaHistoryService mediaHistoryService;
        private ObservableCollection<DataGridColumn> _columnCollection = new ObservableCollection<DataGridColumn>();
        private ObservableCollection<dynamic> dataItems = new ObservableCollection<dynamic>();
        public ObservableCollection<DataGridColumn> ColumnCollection
        {
            get
            {
                return this._columnCollection;
            }
            set
            {
                SetProperty(ref _columnCollection, value);
            }
        }
        public DataViewModel(IMediaHistoryService mediaHistoryService)
        {
            this.mediaHistoryService = mediaHistoryService;
        }



        #region Props

        private bool iswaiting;
        public bool IsWaiting
        {
            get { return iswaiting; }
            set
            {
                iswaiting = value;
                RaisePropertyChanged("IsWaiting");
            }
        }

        public ObservableCollection<dynamic> DataItems
        {
            get
            {
                return this.dataItems;
            }
            set
            {
                SetProperty(ref dataItems, value);
            }
        }

        #endregion

        #region Commands
        public ICommand LoadCommand => new DelegateCommand((obj) =>
        {
            IsWaiting = true;


            Task.Run( () =>
            {
                return mediaHistoryService.GetAll();
            }).ContinueWith(token =>
            {
                IsWaiting = false;
                if (token.IsFaulted) throw token.Exception;
                DataItems.Clear();
                foreach (var item in token.Result)
                {
                    dynamic expando = item;
                    //var dictionary = (IDictionary<string, object>)expando;

                    //foreach (var property in item.GetType().GetProperties())
                    //    dictionary.Add(property.Name, property.GetValue(item));


                    DataItems.Add(expando);
                }
             
            }, TaskScheduler.FromCurrentSynchronizationContext());
        });


        #endregion

    }
}
