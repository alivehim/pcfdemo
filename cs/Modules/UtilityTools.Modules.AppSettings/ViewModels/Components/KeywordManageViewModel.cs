using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class KeywordManageViewModel : BindableBase
    {
        private string keyword;
        private int star;
        private int id;
        private readonly IMediaKeywordService mediaKeywordService;
        private KeywordItemViewModel selectItem;
        public KeywordManageViewModel(IMediaKeywordService mediaKeywordService)
        {
            this.mediaKeywordService = mediaKeywordService;

            Add = new DelegateCommand((obj) =>
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    mediaKeywordService.Add(new Data.Domain.MediaKeyword { Keyword = keyword, Star = Star });
                    LoadData();

                    //var eventAggregator = ToolsContext.Current.UnityContainer.ResolveService<IEventAggregator>();
                    //eventAggregator.GetEvent<MediaKeyChangedEvent>().Publish();

                    ToolsContext.Current.PublishEvent<MediaKeyChangedEvent>();
                }
            });

            Delete = new DelegateCommand((obj) =>
            {
                var item = obj as KeywordItemViewModel;
                mediaKeywordService.DeleteByName(item.Keyword);
                LoadData();
                ToolsContext.Current.PublishEvent<MediaKeyChangedEvent>();
            });

            Select = new DelegateCommand((obj) =>
            {
                var selectitem = obj as KeywordItemViewModel;
                Keyword = selectitem?.Keyword;
                Star = selectitem?.Star ?? 0;

                Id = selectitem?.Id??0;

                Clipboard.SetDataObject(selectitem?.Keyword??"");
            });

            Update = new DelegateCommand((obj) =>
            {
                mediaKeywordService.Update(new Data.Domain.MediaKeyword { Id=Id, Keyword = keyword, Star = Star });
                LoadData();


                ToolsContext.Current.PublishEvent<MediaKeyChangedEvent>();
            });

            LoadData();
        }

        public ICommand Add { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Save { get; set; }

        public ICommand Select { get; set; }

        public ICommand Update { get; set; }


        public ObservableCollection<KeywordItemViewModel> Items { get; set; } = new ObservableCollection<KeywordItemViewModel>();

        public string Keyword
        {
            get
            {
                return keyword;
            }
            set
            {
                keyword = value;
                RaisePropertyChanged("Keyword");
            }

        }

        public int Star
        {
            get
            {
                return star;
            }
            set
            {
                star = value;
                RaisePropertyChanged("Star");
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                RaisePropertyChanged("Id");
            }
        }


        public KeywordItemViewModel SelectItem
        {
            get
            {
                return selectItem;
            }
            set
            {
                selectItem = value;
                RaisePropertyChanged("SelectItem");
            }
        }




        private void LoadData()
        {
            Items.Clear();

            var items = mediaKeywordService.GetAll();

            foreach (var item in items)
            {
                Items.Add(new KeywordItemViewModel { Keyword = item.Keyword, Star = item.Star ?? 0, Id = item.Id }); ;
            }
        }

    }
}
