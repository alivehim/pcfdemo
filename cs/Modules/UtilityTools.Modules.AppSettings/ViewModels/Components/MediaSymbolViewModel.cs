using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using UniversalFramework.Core.Extensions;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.UX;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class MediaSymbolViewModel : ViewModelBase
    {
        private string name;
        private string _value;

        private string path;

        private readonly IMediaSymbolDBService _mediaSymbolDBService;
        private SearchDropdownItem selectedDropdownItem;
        public ObservableCollection<SearchDropdownItem> symbolTypeItems;


     

        public MediaSymbolViewModel(IMediaSymbolDBService mediaSymbolDBService)
        {
            _mediaSymbolDBService = mediaSymbolDBService;

            Add = new DelegateCommand((obj) =>
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(_value))
                {
                    _mediaSymbolDBService.Add(new Data.Domain.MediaSymbol
                    {
                        Symbol = name,
                        Address = _value,
                        StoragePath = Path
                    });
                    LoadData();
                }
            });
            Delete = new DelegateCommand((obj) =>
            {
                var item = obj as MediaSymbolItemViewModel;
                _mediaSymbolDBService.DeleteByName(item.Name);
                LoadData();
            });

            Update = new DelegateCommand((obj) =>
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(_value))
                {
                    _mediaSymbolDBService.Update(new Data.Domain.MediaSymbol
                    {
                        Symbol = name,
                        Address = _value,
                        StoragePath = Path
                    });
                    LoadData();
                }
            });

            LoadData();
            InitSymbolTtype();

            SetDataSource();
        }

        public ICommand Add { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Save { get; set; }

        public ICommand Update { get; set; }

        public ICommand MouseDoubleCommand => new DelegateCommand((obj) =>
            {
                var selectitem = obj as MediaSymbolItemViewModel;

                CurrentDropdownItem= SymbolTypeItems.Single(p=>p.Name== selectitem.Name);
                Name = selectitem.Name;
                Value = selectitem.Value;

                Path = selectitem.Path;

                Clipboard.SetDataObject(selectitem.Value);
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
        public ObservableCollection<MediaSymbolItemViewModel> Items { get; set; } = new ObservableCollection<MediaSymbolItemViewModel>();

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

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
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

        private void LoadData()
        {
            Items.Clear();

            var items = _mediaSymbolDBService.GetAll();

            foreach (var item in items)
            {
                Items.Add(new MediaSymbolItemViewModel { Name = item.Symbol, Value = item.Address, Path = item.StoragePath });
            }
        }

        private void InitSymbolTtype()
        {
            SymbolTypeItems = new ObservableCollection<SearchDropdownItem>();
            var items = EnumExtensions.GetDropdownItems<MediaSymbolType>();

            foreach (var item in items)
            {
                SymbolTypeItems.Add(item);
            }

            CurrentDropdownItem = SymbolTypeItems[0];
        }

        protected void SetDataSource()
        {
            PageListViewSource.Source = Items;
            PageListViewSource.Filter += new FilterEventHandler(ViewSrcOnFileter);

        }

        private void ViewSrcOnFileter(object sender, FilterEventArgs e)
        {
            e.Accepted = PageNameFilter((MediaSymbolItemViewModel)e.Item);
        }


        protected bool PageNameFilter(MediaSymbolItemViewModel description)
        {
            if (string.IsNullOrEmpty(NameFilter))
                return true;


            return description.Name?.Contains(NameFilter ?? "") ?? false;
        }


    }
}
