using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Core.Models.UX;
using UtilityTools.Core.Models.UX.ErrorList;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Core.Mvvm
{
    public abstract class BaseModuleViewModel<T> : ViewModelBase where T : BaseUXItemDescription
    {

        #region private variables

        private SearchDropdownItem selectedDropdownItem=new SearchDropdownItem();
        private string url;
        private bool iswaiting = false;
        protected readonly IModuleMessageRepository moduleMessageRepository;
        protected readonly IUXUpdateMessageRepository uxUpdateMessageRepository;

        //private readonly ILogMessageRepository logMessageRepository;
        //private readonly IUXUpdateMessageRepository uXUpdateMessageRepository;


        protected readonly ISearchHistoryService searchHistoryService;
        protected readonly ITaskManager taskManager;
        protected readonly IEventAggregator eventAggregator;
        protected readonly IContainerProvider containerProvider;


        private CollectionViewSource pageListViewSource = new CollectionViewSource();
        private CollectionViewSource locationListViewSource = new CollectionViewSource();
        private ObservableCollection<ErrorListDataEntry> logItems = new ObservableCollection<ErrorListDataEntry>();

        #endregion

        protected BaseModuleViewModel(IModuleMessageRepository moduleMessageRepository,
            IUXUpdateMessageRepository uxUpdateMessageRepository,
            //IUXUpdateMessageRepository uXUpdateMessageRepository,
            ISearchHistoryService searchHistoryService,
            ITaskManager taskManager, IEventAggregator eventAggregator,
            IContainerProvider containerProvider)
        {
            this.moduleMessageRepository = moduleMessageRepository;
            this.uxUpdateMessageRepository = uxUpdateMessageRepository;
            //this.uXUpdateMessageRepository = uXUpdateMessageRepository;
            this.searchHistoryService = searchHistoryService;
            this.taskManager = taskManager;
            this.eventAggregator = eventAggregator;
            this.containerProvider = containerProvider;

            InitComponent();
            InitHistoricalData();

            SetDataSource();
            this.eventAggregator = eventAggregator;

            ModuleId = Guid.NewGuid().ToString();
        }



        #region Props


   

        private CollectionViewSource PageListViewSource
        {
            get { return pageListViewSource; }
            set { SetProperty(ref pageListViewSource, value); }
        }

        private CollectionViewSource LocationViewSource
        {
            get { return locationListViewSource; }
            set { SetProperty(ref locationListViewSource, value); }
        }

        public ICollectionView PageListView
        {
            get
            {
                return PageListViewSource.View;
            }
        }

        public ICollectionView LocationView
        {
            get
            {
                return LocationViewSource.View;
            }
        }

        public string ModuleId { get; set; }


        public ObservableCollection<T> PageList { get; set; } = new ObservableCollection<T>();

        /// <summary>
        /// 历史记录
        /// </summary>
        public ObservableCollection<SearchDropdownItem> HistoryItems { get; set; } = new ObservableCollection<SearchDropdownItem>();


        public ObservableCollection<ErrorListDataEntry> LogItems
        {
            get { return logItems; }
            set { SetProperty(ref logItems, value); }
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                RaisePropertyChanged("Url");
            }

        }

        private string extendFilter;
        public string ExtendFilter
        {
            get
            {
                return extendFilter;
            }
            set
            {
                extendFilter = value;
                RaisePropertyChanged("ExtendFilter");
            }

        }

        /// <summary>
        /// 当前选择项
        /// </summary>
        public SearchDropdownItem CurrentDropdownItem
        {
            get
            {
                return this.selectedDropdownItem;
            }
            set
            {
                //if (selectedDropdownItem != null && !this.selectedDropdownItem.Equals(value))
                {
                    this.selectedDropdownItem = value;
                    base.RaisePropertyChanged("CurrentDropdownItem");
                }
                //else
                //{

                //}
            }
        }

        /// <summary>
        /// 是否等待
        /// </summary>
        public bool IsWaiting
        {
            get { return iswaiting; }
            set
            {
                iswaiting = value;
                RaisePropertyChanged("IsWaiting");
            }
        }


        public ITaskManager TaskManager => taskManager;
        public CancellationTokenSource CancelTokenSource { get; } = new CancellationTokenSource();


        #endregion


        #region Commands

        public ICommand SelectedItemChangedCommand => new DelegateCommand((selectedItem) =>
        {
            //Console.WriteLine(selectedItem);

            if (selectedItem != null)
            {
                var item = selectedItem as BaseUXItemDescription;
                if (item != null)
                {
                    LogItems = item.MessageListData;

                    ItemSelectedChanged(item);
                }
                else
                {
                    var vm = selectedItem as ViewModelBase;
                    ItemSelectedChanged(vm);
                }

            }
        });

        public ICommand DoubleClickCommand => new DelegateCommand((selectedItem) =>
        {
            //Console.WriteLine(selectedItem);

            if (selectedItem != null)
            {
                var item = selectedItem as BaseUXItemDescription;
                LogItems = item.MessageListData;

                DoubleClickItem(item);
            }
        });

        #endregion

        public MessageOwner GetMessageOwner()
        {
            var type = this.GetType();
            var attribute = type.GetCustomAttribute<MessageOwnerAttribute>();
            return attribute.MessageOwner;
        }

        private void InitComponent()
        {
            moduleMessageRepository.Subscribe((data) =>
            {
                if (data.MessageOwner == GetMessageOwner())
                {
                    UpdateItems(data);
                    IsWaiting = false;
                }
            });

            uxUpdateMessageRepository.Subscribe((data) =>
            {
                if (data.MessageOwner == GetMessageOwner())
                {
                    UpdateMessage(data);
                }
            });


            //uXUpdateMessageRepository.Subscribe((data) =>
            //{
            //    if (data.MessageOwner == GetMessageOwner())
            //    {
            //        UpdateMessage(data);
            //    }
            //});
        }


        /// <summary>
        /// 加载历史记录
        /// </summary>
        protected virtual void InitHistoricalData()
        {

            this.selectedDropdownItem = new SearchDropdownItem();
            var searchList = searchHistoryService.GetList(GetMessageOwner().ToString(), 0, 100);
            if (searchList != null && searchList.Count > 0)
            {
                foreach (var item in searchList)
                {
                    HistoryItems.Add(new SearchDropdownItem
                    {
                        Name = item.Url,
                        Value = item.Url,
                        ModifyTime = item.LatestUsedTime,
                        Icon = !item.Url.Contains(@":\")? new BitmapImage(new Uri($@"/UtilityTools.Modules.MediaGet;component/Resources/Icons/website.ico", UriKind.Relative)):
                        new BitmapImage(new Uri($@"/UtilityTools.Modules.MediaGet;component/Resources/Images/folder.png", UriKind.Relative))
                    });
                }

                CurrentDropdownItem = HistoryItems[0];
            }
        }

        protected virtual void ItemSelectedChanged(BaseUXItemDescription data)
        {

        }

        protected virtual void ItemSelectedChanged(ViewModelBase  viewModelBase)
        {

        }

        protected virtual void DoubleClickItem(BaseUXItemDescription data)
        {

        }

        protected abstract void UpdateItems(IExtractResult<BaseResourceMetadata> data);

        protected virtual void UpdateUX(IUXMessage uXMessage)
        {

        }

        protected virtual void UpdateMessage(IUXMessage uxMessage)
        {
            if (uxMessage is ContentLogMetadata log)
            {
                var item = PageList.FirstOrDefault(p => p.ID == log.ID);
                if (item != null)
                {
                    switch (log.ErrorLevel)
                    {
                        case ErrorListLevel.Error:
                            item.MessageListData.Add(new ErrorListDataEntry { Level = ErrorListLevel.Error, Description = log.Message });
                            break;
                        case ErrorListLevel.Information:
                            item.MessageListData.Add(new ErrorListDataEntry { Level = ErrorListLevel.Information, Description = log.Message });
                            break;
                        case ErrorListLevel.Warning:
                            item.MessageListData.Add(new ErrorListDataEntry { Level = ErrorListLevel.Warning, Description = log.Message });
                            break;
                    }
                }
            }

            UpdateUX(uxMessage);

        }
        protected virtual void UpdateItemsAfter() { }

        protected virtual bool PageNameFilter(T ob) { return true; }

        protected virtual bool LocationNameFilter(SearchDropdownItem ob) { return true; }

        public void SearchHistory2DB(string url)
        {
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                if (searchHistoryService.Save(new SearchHistoryItem
                {
                    CreatedTime = DateTime.Now,
                    Owner = GetMessageOwner().ToString(),
                    Url = url,
                }) > 0)
                {
                    HistoryItems.Insert(0, new SearchDropdownItem { Name = url, Value = url, ModifyTime = DateTime.Now });
                }
                else
                {

                    var items = HistoryItems.Where(p => p.Value == Url).ToList();
                    for (var i = 0; i < items.Count; i++)
                    {
                        HistoryItems.Remove(items[i]);
                    }

                    HistoryItems.Insert(0, new SearchDropdownItem { Name = url, Value = url, ModifyTime = DateTime.Now });
                }

                Url = url;
            }));


        }

        private void ViewSrcOnFileter(object sender, FilterEventArgs e)
        {
            e.Accepted = PageNameFilter((T)e.Item);
        }

        private void LocatinViewSrcOnFileter(object sender, FilterEventArgs e)
        {
            e.Accepted = LocationNameFilter((SearchDropdownItem)e.Item);
        }

        public void RefreshPageData()
        {
            PageListViewSource.View.Refresh();
        }

        public void RefreshLocationData()
        {
            LocationViewSource.View.Refresh();
        }

        protected void SetDataSource()
        {
            PageListViewSource.Source = PageList;
            PageListViewSource.Filter += new FilterEventHandler(ViewSrcOnFileter);

            LocationViewSource.Source = HistoryItems;
            LocationViewSource.Filter += new FilterEventHandler(LocatinViewSrcOnFileter);
        }

        public void Info(string message)
        {

            var messageprovider = containerProvider.ResolveService<IMessageStreamProvider<IUXMessage>>();

            messageprovider.Info(message);
        }

        public void Error(string message)
        {
            var messageprovider = containerProvider.ResolveService<IMessageStreamProvider<IUXMessage>>();

            messageprovider.Error(message);
        }

        public void Error(Exception message)
        {
            var messageprovider = containerProvider.ResolveService<IMessageStreamProvider<IUXMessage>>();

            messageprovider.Error(message.Message);
        }
    }
}
