using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using UtilityTools.Core.Definition;
using UtilityTools.Core.Events;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Utilites;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Views;

namespace UtilityTools.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "UtilityTools Application";
        private readonly IUXUpdateMessageRepository logMessageRepository;

        private ObservableCollection<GlobalLogItemViewModel> logItems = new ObservableCollection<GlobalLogItemViewModel>();

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<GlobalLogItemViewModel> LogItems
        {
            get { return logItems; }
            set { SetProperty(ref logItems, value); }
        }

        public MainWindowViewModel(IUXUpdateMessageRepository logMessageRepository)
        {
            this.logMessageRepository = logMessageRepository;
            logMessageRepository.Subscribe((data) =>
            {
                if (data.MessageOwner == Core.Models.MessageOwner.None)
                {
                    UpdateMessage(data);
                }
            });
        }

        public ICommand JumptoWebResourceCommand => new DelegateCommand(() =>
        {

            ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>(ModuleName.WebResouce.ToString());
            //App.Current.MainWindow.Activate();
            //Application.Current.MainWindow.Show();

            WindowsHelper.ShowWindow();
        });

        public ICommand JumptoMediaCommand => new DelegateCommand(() =>
        {
            //Application.Current.MainWindow.Show();
            ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>(ModuleName.MediaGet.ToString());

            WindowsHelper.ShowWindow();
        });

        public ICommand JumptoCanvasCommand => new DelegateCommand(() =>
        {
            //Application.Current.MainWindow.Show();
            ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>(ModuleName.Canvas.ToString());

            WindowsHelper.ShowWindow();
        });

        public ICommand JumptoAutoflowCommand => new DelegateCommand(() =>
        {
            //Application.Current.MainWindow.Show();
            ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>(ModuleName.Canvas.ToString());
            ToolsContext.Current.PublishEvent<WidgeModuleChange, string>(WidgetModuleName.AutomateFlowRegion.ToString());

            WindowsHelper.ShowWindow();
        });

        public ICommand JumptoXMLCommand => new DelegateCommand(() =>
        {
            //Application.Current.MainWindow.Show();
            ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>(ModuleName.Canvas.ToString());
            ToolsContext.Current.PublishEvent<WidgeModuleChange, string>(WidgetModuleName.XmlRegion.ToString());
            WindowsHelper.ShowWindow();
        });

        public ICommand JumptoDictionaryCommand => new DelegateCommand(() =>
        {
            //Application.Current.MainWindow.Show();
            ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>(ModuleName.Canvas.ToString());
            ToolsContext.Current.PublishEvent<WidgeModuleChange, string>(WidgetModuleName.DictionaryRegion.ToString());

            WindowsHelper.ShowWindow();
        });

        public ICommand JumptoTTSCommand => new DelegateCommand(() =>
        {
            //Application.Current.MainWindow.Show();
            ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>(ModuleName.Canvas.ToString());
            ToolsContext.Current.PublishEvent<WidgeModuleChange, string>(WidgetModuleName.TexttoSpeech.ToString());

            WindowsHelper.ShowWindow();
        });

        protected void UpdateMessage(IUXMessage data)
        {
            var log = data as ContentLogMetadata;
            LogItems.Add(new GlobalLogItemViewModel { Message = log.Message, Level = log.ErrorLevel });
        }
    }
}
