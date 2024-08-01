using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using UniversalFramework.Core.Extensions;
using UtilityTools.Core.Models.UX;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Core.Mvvm;
using System.Windows;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Events;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class MediaHubConfigrationViewModel :  ViewModelBase
    {

        private string name;
        private string address;

        private string path;

        private readonly ISellerHubService  sellerHubService;
        private SearchDropdownItem selectedDropdownItem;
        public ObservableCollection<SearchDropdownItem> symbolTypeItems;




        public MediaHubConfigrationViewModel(ISellerHubService  sellerHubService)
        {
            this.sellerHubService = sellerHubService;

            Add = new DelegateCommand((obj) =>
            {
                if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(Path))
                {
                    sellerHubService.Add(new Data.Domain.SellerHub
                    {
                        SellerName = Name,
                        Address = Address,
                        StoragePath = Path
                    });
                    LoadData();
                }
            });
            Delete = new DelegateCommand((obj) =>
            {
                var item = obj as MediaHubConfigrationItemViewModel;
                sellerHubService.DeleteByName(item.Seller);
                LoadData();
            });

            Update = new DelegateCommand((obj) =>
            {
                if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(Path))
                {
                    sellerHubService.Update(new Data.Domain.SellerHub
                    {
                        SellerName = name,
                        Address = Address,
                        StoragePath = Path
                    });
                    LoadData();
                }
            });

            LoadData(false);

            SetDataSource();
        }

        public ICommand Add { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Save { get; set; }

        public ICommand Update { get; set; }

        public ICommand MouseDoubleCommand => new DelegateCommand((obj) =>
        {
            var selectitem = obj as MediaHubConfigrationItemViewModel;

            Name = selectitem.Seller;
            Address = selectitem.Address;

            Path = selectitem.Path;

            Clipboard.SetDataObject(selectitem.Address);
        });

        public ObservableCollection<SearchDropdownItem> SymbolTypeItems
        {
            get { return symbolTypeItems; }
            set
            {
                symbolTypeItems = value;
                RaisePropertyChanged("SymbolTypeItems");
            }
        }

        private CollectionViewSource pageListViewSource = new CollectionViewSource();
        public ObservableCollection<MediaHubConfigrationItemViewModel> Items { get; set; } = new ObservableCollection<MediaHubConfigrationItemViewModel>();

        private CollectionViewSource PageListViewSource
        {
            get { return pageListViewSource; }
            set { SetProperty(ref pageListViewSource, value); }
        }


        public ICollectionView PageListView
        {
            get
            {
                return PageListViewSource.View;
            }
        }

        private string nameFilter;
        public string NameFilter
        {
            get
            {
                return nameFilter;
            }
            set
            {
                nameFilter = value;
                RaisePropertyChanged("NameFilter");

                PageListViewSource.View.Refresh();
            }
        }


        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }

        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                RaisePropertyChanged("Address");
            }

        }

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                RaisePropertyChanged("Path");
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
                if (selectedDropdownItem == null)
                {
                    this.selectedDropdownItem = value;
                    base.RaisePropertyChanged("CurrentDropdownItem");
                }
                else if (selectedDropdownItem != null && !this.selectedDropdownItem.Equals(value))
                {
                    this.selectedDropdownItem = value;
                    base.RaisePropertyChanged("CurrentDropdownItem");
                }

            }
        }

        private void LoadData(bool needSyncData=true)
        {
            Items.Clear();

            var items = sellerHubService.GetAll();

            foreach (var item in items)
            {
                Items.Add(new MediaHubConfigrationItemViewModel { Seller = item.SellerName, Address = item.Address, Path = item.StoragePath });
            }

            if(needSyncData)
            {
                ToolsContext.Current.PublishEvent<UpateSellerEvent>();
            }
        }


        protected void SetDataSource()
        {
            PageListViewSource.Source = Items;
            PageListViewSource.Filter += new FilterEventHandler(ViewSrcOnFileter);

        }

        private void ViewSrcOnFileter(object sender, FilterEventArgs e)
        {
            e.Accepted = PageNameFilter((MediaHubConfigrationItemViewModel)e.Item);
        }


        protected bool PageNameFilter(MediaHubConfigrationItemViewModel description)
        {
            if (string.IsNullOrEmpty(NameFilter))
                return true;


            return description.Seller?.Contains(NameFilter ?? "") ?? false;
        }

    }
}
