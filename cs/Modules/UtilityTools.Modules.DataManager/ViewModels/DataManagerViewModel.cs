using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Events;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.D365;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Core.Mvvm;
using UtilityTools.Modules.DataManager.ViewModels.Commands;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Modules.DataManager.ViewModels
{
    [MessageOwner(MessageOwner.DataManager)]
    public class DataManagerViewModel : BaseModuleViewModel<RequestDescription>
    {
        private string nameFilter;
        private string token;
        public ICommand Load { get; set; }
        public ICommand MockData { get; set; }
        public DataManagerViewModel(IModuleMessageRepository moduleMessageRepository, ILogMessageRepository logMessageRepository, ISearchHistoryService searchHistoryService, ITaskManager taskManager, IEventAggregator eventAggregator, IContainerProvider containerProvider) : base(moduleMessageRepository, logMessageRepository, searchHistoryService, taskManager, eventAggregator, containerProvider)
        {
            Load = new LoadCommand(this);
            MockData = new MockDataCommand(this);
            InitToken();

            eventAggregator.GetEvent<TokenEvent>().Subscribe(ShowToken);
        }

        /// <summary>
        /// the authorizion token to call dynamic api
        /// </summary>
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
                RaisePropertyChanged("Token");
            }
        }


        /// <summary>
        /// the string to filter the data
        /// </summary>
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
                RefreshPageData();
            }
        }

        /// <summary>
        /// get the token from db
        /// </summary>
        protected void InitToken()
        {

            var token = Settings.Current.D365AccessToken;

            if (!string.IsNullOrEmpty(token))
            {
                Token = token;
            }
        }

        protected override void UpdateItems(IExtractResult<BaseResourceMetadata> data)
        {
            if (data is ExtractResult<ViolaRequestDescription>)
            {
                var page = data as ExtractResult<ViolaRequestDescription>;
                IsWaiting = false;
                foreach (var item in page.Collection)
                {
                    try
                    {
                        var leadtimeReminder = string.Empty;
                        if (item.aia_sys_leadtime_reminder.HasValue)
                        {
                            var reminder = (LeaderReminder)item.aia_sys_leadtime_reminder;
                            leadtimeReminder = reminder.ToString();
                        }
                        var vItem = new RequestDescription
                        {
                            Name = item.Name,
                            FormId = item.formId,
                            MessageOwner = MessageOwner.DataManager,
                            WFRequestId=item.wf_request_id,
                            Priority= item.aia_com_priority == 143140000? "Urgent":"Normal",
                            LeadtimeReminder= leadtimeReminder
                        };

                        PageList.Add(vItem);
                    }
                    catch (Exception ex)
                    {
                        Error(ex.ToString());
                    }
                }

                RefreshPageData();
            }
        }

        /// <summary>
        /// filter the data
        /// </summary>
        /// <param name="description">the entity need to be filter</param>
        /// <returns></returns>
        protected override bool PageNameFilter(RequestDescription description)
        {
            return description.Name.Contains(NameFilter ?? "");
        }

        private void ShowToken(string token)
        {
            Token = token;
        }
    }
}
