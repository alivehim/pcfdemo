using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class BookKeywordManageViewModel : BindableBase
    {
        private string keyword;

        private readonly IMediaKeywordService mediaKeywordService;

        public BookKeywordManageViewModel(IMediaKeywordService mediaKeywordService)
        {
            this.mediaKeywordService = mediaKeywordService;

            Add = new DelegateCommand((obj) =>
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    mediaKeywordService.Add(new Data.Domain.MediaKeyword { Keyword = keyword, Type = 1 });
                    LoadData();
                }
            });

            Delete = new DelegateCommand((obj) =>
            {
                var item = obj as KeywordItemViewModel;
                mediaKeywordService.DeleteByName(item.Keyword, 1);
                LoadData();
            });

            LoadData();
        }

        public ICommand Add { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Save { get; set; }


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




        private void LoadData()
        {
            Items.Clear();

            var items = mediaKeywordService.GetAll(type: 1);

            foreach (var item in items)
            {
                Items.Add(new KeywordItemViewModel { Keyword = item.Keyword }); ;
            }
        }
    }
}
