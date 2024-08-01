using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UtilityTools.CEF;
using UtilityTools.Core.Events;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.CloudService.Vocabulary;

namespace UtilityTools.Modules.Canvas.ViewModels
{
    public class DictionaryWidgetViewModel : BindableBase
    {
        private readonly IDictionaryService dictionaryService;

        private string vocabulary;
        public string Vocabulary
        {
            get
            {
                return vocabulary;
            }
            set
            {
                vocabulary = value;
                RaisePropertyChanged("Vocabulary");
            }
        }

        private string conent;
        public string Content
        {
            get
            {
                return conent;
            }
            set
            {
                conent = value;
                RaisePropertyChanged("Content");
            }
        }

        public DictionaryWidgetViewModel(IDictionaryService dictionaryService)
        {
            this.dictionaryService = dictionaryService;
        }

        public ICommand GetBasicConceptCommand => new DelegateCommand(async () =>
        {
            Content = await dictionaryService.GetBasicConceptAsync(Vocabulary);
        });

        public ICommand GetExampleCommand => new DelegateCommand(async () =>
        {
            Content = await dictionaryService.GetExampleAsync(Vocabulary);
        });

        public ICommand GetVariantsCommand => new DelegateCommand(async () =>
        {
            Content = await dictionaryService.GetVariantsAsync(Vocabulary);
        });

        public ICommand GetPhraseCommand => new DelegateCommand(async () =>
        {
            Content = await dictionaryService.GetPhraseAsync(Vocabulary);
        });

        public ICommand GetWordRootCommand => new DelegateCommand(async () =>
        {
            Content = await dictionaryService.GetWordRootAsync(Vocabulary);
        });

        public ICommand OpenBaiduCommand => new DelegateCommand(() =>
        {
            //(new BaiduTranslateWindow(Vocabulary)).ShowDialog();

            //navigate to browser module

            //

            ToolsContext.Current.PublishEvent<ChangeBrowserAddressEvent, BrowserChange>(new BrowserChange
            {
                Url = "https://fanyi.baidu.com/#en/zh/"
            });
        });

        public ICommand OpenBingCommand => new DelegateCommand(() =>
        {
            ToolsContext.Current.PublishEvent<ChangeBrowserAddressEvent, BrowserChange>(new BrowserChange
            {
                Url = "https://www.bing.com/dict/"
            });
        });
    }
}
