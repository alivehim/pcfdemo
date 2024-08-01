using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Events;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.UX.ErrorList;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Modules.PowerPlatform.ViewModels
{
    internal class PowerPlatformViewModel : BaseUXItemDescription
    {
        private ObservableCollection<ErrorListDataEntry> logItems = new ObservableCollection<ErrorListDataEntry>();

        protected readonly IModuleMessageRepository moduleMessageRepository;
        protected readonly IUXUpdateMessageRepository uxUpdateMessageRepository;
        private readonly IEventAggregator eventAggregator;

        public PowerPlatformViewModel(IModuleMessageRepository moduleMessageRepository, IUXUpdateMessageRepository uxUpdateMessageRepository, IEventAggregator eventAggregator)
        {
            this.moduleMessageRepository = moduleMessageRepository;
            this.uxUpdateMessageRepository = uxUpdateMessageRepository;
            this.eventAggregator = eventAggregator;

            eventAggregator.GetEvent<NavigatePowerPlatformEvent>().Subscribe((address) =>
            {
                //if (string.IsNullOrEmpty(Address))
                {

                    Address = address;
                }
                ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>("PowerPlatform");
            });

            InitComponent();
        }

        public ObservableCollection<ErrorListDataEntry> LogItems
        {
            get { return logItems; }
            set { SetProperty(ref logItems, value); }
        }


        private string address = string.Empty;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChanged("Address");
            }
        }

        private void InitComponent()
        {

            uxUpdateMessageRepository.Subscribe((data) =>
            {
                if (data.MessageOwner == MessageOwner.PowerPlatform)
                {
                    UpdateMessage(data);
                }
            });

        }

        protected  void UpdateMessage(IUXMessage uxMessage)
        {
            if (uxMessage is ContentLogMetadata log)
            {
                switch (log.ErrorLevel)
                {
                    case ErrorListLevel.Error:
                        LogItems.Add(new ErrorListDataEntry { Level = ErrorListLevel.Error, Description = log.Message });
                        break;
                    case ErrorListLevel.Information:
                        LogItems.Add(new ErrorListDataEntry { Level = ErrorListLevel.Information, Description = log.Message });
                        break;
                    case ErrorListLevel.Warning:
                        LogItems.Add(new ErrorListDataEntry { Level = ErrorListLevel.Warning, Description = log.Message });
                        break;
                }
            }


        }
    }
}
