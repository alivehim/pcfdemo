using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.D365;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Core.Models.UX;
using UtilityTools.Core.Models.UX.ErrorList;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;
using UtilityTools.Modules.AutomateFlow.ViewModels;
using System.Windows;
using System.Dynamic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace UtilityTools.Modules.AutomateFlow.ViewModels
{
    [MessageOwner(MessageOwner.AutomateFlow)]
    public class AutomateFlowViewModel : BaseModuleViewModel<FlowUXItemDescription>
    {
        #region private variables
        private string d365AccessToken;
        private string nameFilter;
        private WebResourceMetaDefinition webresouceMatadata;
        private SearchDropdownItem selectedSolutionDropdownItem = new SearchDropdownItem();

        private FlowUXItemDescription selectFlowUXItemDescrition = new FlowUXItemDescription();

        private ObservableCollection<DataGridColumn> _columnCollection = new ObservableCollection<DataGridColumn>();
        private readonly string[] ColumnsName = new string[] { "StartTime", "EndTime", "Status" };
        private IList<string> fileChangeList = null;

        private FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        private readonly IEntityDefinitionService entityDefinitionService;
        private readonly IFlowService flowService;
        private ObservableCollection<ExpandoObject> flowDetailItems = new ObservableCollection<ExpandoObject>();

        #endregion

        public AutomateFlowViewModel(IFlowService flowService, IEntityDefinitionService entityDefinitionService, IModuleMessageRepository moduleMessageRepository,
            IUXUpdateMessageRepository uXUpdateMessageRepository,
            ISearchHistoryService searchHistoryService,
            ITaskManager taskManager,
            IEventAggregator eventAggregator,
            IContainerProvider containerProvider
            ) :
            base(moduleMessageRepository, uXUpdateMessageRepository, searchHistoryService, taskManager, eventAggregator, containerProvider)
        {
            this.entityDefinitionService = entityDefinitionService;
            this.flowService = flowService;


            this.ColumnCollection.Add(new DataGridTextColumn()
            {
                Header = "StartTime",
                Binding = new Binding("StartTime")
            });

            this.ColumnCollection.Add(new DataGridTextColumn()
            {
                Header = "EndTime",
                Binding = new Binding("EndTime")
            });

            this.ColumnCollection.Add(new DataGridTextColumn()
            {
                Header = "Status",
                Binding = new Binding("Status")
            });

        }

        #region Props

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

        public ObservableCollection<SearchDropdownItem> SolutionItems { get; set; } = new ObservableCollection<SearchDropdownItem>();
        public ObservableCollection<DependencyMetadataInfoViewModel> RequiredDependencyMetadataInfos { get; set; } = new ObservableCollection<DependencyMetadataInfoViewModel>();
        public ObservableCollection<DependencyMetadataInfoViewModel> DependencyMetadataInfos { get; set; } = new ObservableCollection<DependencyMetadataInfoViewModel>();
        public ObservableCollection<FlowColumnViewModel> CustomColumns { get; set; } = new ObservableCollection<FlowColumnViewModel>();
        public ObservableCollection<ExpandoObject> FlowDetailItems
        {
            get
            {
                return this.flowDetailItems;
            }
            set
            {
                SetProperty(ref flowDetailItems, value);
            }
        }


        public IList<string> FileChangeList
        {
            get { return fileChangeList; }
            set
            {
                fileChangeList = value;
                RefreshPageData();
            }
        }

        /// <summary>
        /// 当前选择项
        /// </summary>
        public SearchDropdownItem CurrentSolutionDropdownItem
        {
            get
            {
                return this.selectedSolutionDropdownItem;
            }
            set
            {
                //if (selectedSolutionDropdownItem != null && !this.selectedSolutionDropdownItem.Equals(value))
                {
                    this.selectedSolutionDropdownItem = value;
                    base.RaisePropertyChanged("CurrentSolutionDropdownItem");

                    Settings.Current.D365SolutionId = value?.Value;
                }
            }
        }
        public FlowUXItemDescription SelectedItem
        {
            get
            {
                return this.selectFlowUXItemDescrition;
            }
            set
            {
                //if (selectedSolutionDropdownItem != null && !this.selectedSolutionDropdownItem.Equals(value))
                {
                    this.selectFlowUXItemDescrition = value;
                    base.RaisePropertyChanged("SelectedItem");

                }
            }
        }
        public string D365AccessToken
        {
            get
            {
                return d365AccessToken;
            }
            set
            {
                d365AccessToken = value;
                RaisePropertyChanged("D365AccessToken");

            }
        }
        private string serviceToken;
        public string ServiceToken
        {
            get
            {
                return serviceToken;
            }
            set
            {
                serviceToken = value;
                RaisePropertyChanged("ServiceToken");

            }
        }
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

        private bool showTokenTextBox;
        public bool ShowTokenTextBox
        {
            get
            {
                return showTokenTextBox;
            }
            set
            {
                showTokenTextBox = value;
                RaisePropertyChanged("ShowTokenTextBox");
            }
        }

        private bool showDependentComponents;
        public bool ShowDependentComponents
        {
            get
            {
                return showDependentComponents;
            }
            set
            {
                showDependentComponents = value;
                RaisePropertyChanged("ShowDependentComponents");
            }
        }


        private bool showFlowHistory;
        public bool ShowFlowHistory
        {
            get
            {
                return showFlowHistory;
            }
            set
            {
                showFlowHistory = value;
                RaisePropertyChanged("ShowFlowHistory");
            }
        }

        private bool customColumnViewEnable;
        public bool CustomColumnViewEnable
        {
            get
            {
                return customColumnViewEnable;
            }
            set
            {
                customColumnViewEnable = value;
                RaisePropertyChanged("CustomColumnViewEnable");
            }
        }


        public string Token => ShowTokenTextBox ? D365AccessToken : Settings.Current.D365AccessToken;

        public string FlowServiceToken => ShowTokenTextBox ? ServiceToken : Settings.Current.FlowToken;

        public string NextLink;

        #endregion

        #region commands
        public ICommand OpenConfigurationCommand => new DelegateCommand((obj) =>
        {
            CustomColumnViewEnable = true;
        });
        public ICommand LoadCommand => new DelegateCommand((obj) =>
        {
            PageList.Clear();
            IsWaiting = true;


            var taskContext = new TaskContext(CancelTokenSource, Core.Models.MessageOwner.AutomateFlow, ModuleId)
            {
                Key = Token
            };

            TaskManager.StartNew(taskContext);
        });

        public ICommand GetRequiredComponentsCommand => new DelegateCommand(async (obj) =>
        {
            var item = obj as FlowUXItemDescription;
            RequiredDependencyMetadataInfos.Clear();
            IsWaiting = true;
            var result = await entityDefinitionService.GetFlowRequiredComponentsAsync(item.ObjectId, Token);

            foreach (var metainfo in result.DependencyMetadataCollection.DependencyMetadataInfoCollection)
            {
                RequiredDependencyMetadataInfos.Add(new DependencyMetadataInfoViewModel
                {
                    Name = metainfo.requiredcomponentdisplayname,
                    Type = metainfo.requiredcomponenttypename,
                    Id = metainfo.requiredcomponentobjectid
                });
            }

            ShowDependentComponents = false;
            ShowFlowHistory = false;
            IsWaiting = false;
        });

        public ICommand GetDependentComponentsCommand => new DelegateCommand(async (obj) =>
        {
            IsWaiting = true;
            var item = obj as FlowUXItemDescription;
            RequiredDependencyMetadataInfos.Clear();
            var result = await entityDefinitionService.GetFlowDependentComponentsAsync(item.ObjectId, Token);

            foreach (var metainfo in result.DependencyMetadataCollection.DependencyMetadataInfoCollection)
            {
                DependencyMetadataInfos.Add(new DependencyMetadataInfoViewModel
                {
                    Name = metainfo.dependentcomponentdisplayname,
                    Type = metainfo.dependentcomponenttypename,
                    Id = metainfo.dependentcomponentobjectid
                });
            }
            ShowDependentComponents = true;
            ShowFlowHistory = false;
            IsWaiting = false;
        });

        public ICommand CopyCommand => new DelegateCommand((obj) =>
        {
            var item = obj as DependencyMetadataInfoViewModel;
            Clipboard.SetDataObject(item.Name);
        });

        public ICommand UpdateColumnsCommand => new DelegateCommand(async (obj) =>
        {

            var list = CustomColumns.Where(p => p.IsChecked);


            //remove columns

            while (ColumnCollection.Count > ColumnsName.Length)
            {
                ColumnCollection.RemoveAt(ColumnCollection.Count - 1);
            }

            foreach (var item in list)
            {
                this.ColumnCollection.Add(new DataGridTextColumn()
                {
                    Header = item.Name,
                    Binding = new Binding(item.Name)
                });
            }

            //update datagrid

            bool isNeedRefresh = true;
            foreach (dynamic item in FlowDetailItems)
            {
                var dic = item as IDictionary<string, object>;
                foreach (var item2 in list)
                {

                    if (dic.ContainsKey(item2.Name))
                    {
                        isNeedRefresh = false;
                    }
                    else
                    {
                        isNeedRefresh = true;
                    }
                }

                if (isNeedRefresh)
                {
                    var detail = await flowService.GetFlowTriggerOutDetail(item.TriggerOutLink);

                    if (detail != null)
                    {
                        var data = detail.body as IDictionary<string, object>;
                        foreach (var item2 in list)
                        {
                            try
                            {

                                dic[item2.Name] = data[item2.Name];
                            }
                            catch
                            {

                            }
                        }

                    }
                }
            }
        });


        public ICommand OpenNextHistoryCommand => new DelegateCommand(async (obj) =>
        {
            var flowItem = obj as FlowUXItemDescription;
            if (!string.IsNullOrEmpty(NextLink))
            {
                IsWaiting = true;

                try
                {
                    var result = await flowService.GetFlowHistoryAsync(NextLink, FlowServiceToken);

                    foreach (var item in result.value)
                    {
                        dynamic data = new ExpandoObject();

                        data.ID = item.id;
                        data.Name = item.name;
                        data.EndTime = DateTime.Parse(item.properties.endTime).AddHours(8);
                        data.StartTime = DateTime.Parse(item.properties.endTime).AddHours(8);
                        data.Status = item.properties.status;
                        data.TriggerOutLink = item.properties.trigger.outputsLink.uri;

                        FlowDetailItems.Add(data);

                        var datadic = data as IDictionary<string, object>;

                        if (CustomColumns.Count(p => p.IsChecked) > 0)
                        {
                            var detail = await flowService.GetFlowTriggerOutDetail(data.TriggerOutLink);
                            //var dic = item as IDictionary<string, object>;
                            if (detail != null)
                            {
                                var attrs = detail.body as IDictionary<string, object>;
                                foreach (var attr in CustomColumns.Where(p => p.IsChecked).ToList())
                                {
                                    try
                                    {

                                        datadic[attr.Name] = attrs[attr.Name];
                                    }
                                    catch
                                    {

                                    }
                                }

                            }
                        }
                    }
                    NextLink = result.nextLink;
                }
                catch
                {

                }
                finally
                {
                    IsWaiting = false;

                }

            }
        });

        public ICommand GetEditUrlCommand => new DelegateCommand((obj) =>
        {
            var item = obj as FlowUXItemDescription;

            //https://make.powerautomate.com/environments/6f93d7ef-77e7-4491-a5d8-a0d4b4d5114f/solutions/9486d209-c82c-ed11-9db2-000d3a0871cb/flows/e2393a83-0388-4805-90a0-0d11416994a6?utm_source=solution_explorer&v3=false&reason=fromV3

            var url = $"https://make.powerautomate.com/environments/{Settings.Current.EnvironmentId}/solutions/{Settings.Current.D365SolutionId}/flows/e2393a83-0388-4805-90a0-0d11416994a6?utm_source=solution_explorer&v3=false&reason=fromV3";

            Clipboard.SetDataObject(url);

        });

        public ICommand GetHistoryDetailCommand => new DelegateCommand((obj) =>
        {
            //https://make.powerautomate.com/environments/6f93d7ef-77e7-4491-a5d8-a0d4b4d5114f/flows/32af331d-2eb6-4e23-9fac-6827caccb260/runs/08584995858399699745025517737CU38
            var item = obj as ExpandoObject;
            //string url = $"{TestUrl}{obj.Name}";

            IDictionary<string, object> dictionary = item as IDictionary<string, object>;

            string url = $"https://make.powerautomate.com/environments/{Settings.Current.EnvironmentId}/flows/{SelectedItem.WorkflowId}/runs/{dictionary["Name"]}";
            Clipboard.SetDataObject(url);
        });

        public ICommand OpenBrowerCommand => new DelegateCommand((obj) =>
        {
            ToolsContext.Current.PublishEvent<NavigatePowerPlatformEvent,string>("https://make.powerapps.com/");
        });
    public ICommand OpenFlowBrowerCommand => new DelegateCommand((obj) =>
        {
            ToolsContext.Current.PublishEvent<NavigatePowerPlatformEvent,string>("https://make.powerautomate.com/");
        });

        #endregion
        #region protected methods

        protected override void DoubleClickItem(BaseUXItemDescription data)
        {
            ShowFlowHistory = true;
            IsWaiting = true;

            FlowDetailItems.Clear();
            CustomColumns.Clear();

            //load history
            Task.Run(async () =>
            {
                var flowItem = data as FlowUXItemDescription;
                return await flowService.GetFlowHistoryAsync(Settings.Current.EnvironmentId, flowItem.WorkflowId, FlowServiceToken);

            }).ContinueWith(async result =>
            {
                IsWaiting = false;
                if (result.IsFaulted) throw result.Exception;

                FlowDetailItems.Clear();

                bool requiredFetchColumn = true;
                foreach (var item in result.Result.value)
                {
                    dynamic data = new ExpandoObject();
                    //{
                    //    ID = item.id,
                    //    Name = item.name,
                    //    EndTime = DateTime.Parse(item.properties.endTime).AddHours(8),
                    //    StartTime = DateTime.Parse(item.properties.endTime).AddHours(8),

                    //    Status = item.properties.status
                    //};
                    data.ID = item.id;
                    data.Name = item.name;
                    data.EndTime = DateTime.Parse(item.properties.endTime).AddHours(8);
                    data.StartTime = DateTime.Parse(item.properties.endTime).AddHours(8);
                    data.Status = item.properties.status;
                    data.TriggerOutLink = item.properties.trigger?.outputsLink?.uri??"";

                    FlowDetailItems.Add(data);

                    if (requiredFetchColumn)
                    {
                        if (!string.IsNullOrEmpty(data.TriggerOutLink))
                        {
                            var detail = await flowService.GetFlowTriggerOutDetail(data.TriggerOutLink);

                            IDictionary<string, object> dictionary = detail.body as IDictionary<string, object>;

                            if (dictionary != null)
                            {
                                foreach (KeyValuePair<string, object> property in dictionary)
                                {
                                    CustomColumns.Add(new FlowColumnViewModel { Name = property.Key, IsChecked = false });
                                }
                            }

                            //foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(detail.body.GetType()))
                            //{
                            //    CustomColumns.Add(property.Name);
                            //}
                            //var expando = detail.body as ExpandoObject;

                            //var expandoDict = expando as IDictionary<string, object>;

                            //if (expandoDict != null)
                            //{
                            //    foreach (var key in expandoDict.Keys)
                            //    {
                            //        CustomColumns.Add(key);
                            //    }
                            //}
                        }

                        requiredFetchColumn = false;
                    }
                }

                NextLink = result.Result.nextLink;


            }, TaskScheduler.FromCurrentSynchronizationContext()); ;


        }

        protected override void UpdateItems(IExtractResult<BaseResourceMetadata> data)
        {

            if (data is ExtractResult<FlowDescription>)
            {
                var page = data as ExtractResult<FlowDescription>;

                PageList.Clear();
                foreach (var item in page.Collection)
                {
                    try
                    {

                        var vItem = new FlowUXItemDescription
                        {
                            Name = item.Name,
                            DisplayName = item.DisplayName,
                            ObjectId = item.ObjectId,
                            WorkflowId = item.WorkflowId,
                            MessageOwner = MessageOwner.AutomateFlow
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


        protected void InitToken()
        {
            //var settingService = ToolsContext.Current.UnityContainer.ResolveService<IUtilityToolsSettingService>();

            var token = Settings.Current.D365AccessToken;

            if (!string.IsNullOrEmpty(token))
            {
                D365AccessToken = token;
            }
        }


        protected override bool PageNameFilter(FlowUXItemDescription description)
        {
            if (string.IsNullOrEmpty(NameFilter))
                return true;


            return (description.Name.ToLower()?.Contains(NameFilter.ToLower() ?? "")) ?? false
                || (description.ObjectId.ToLower()?.Contains(NameFilter.ToLower() ?? "") ?? false)
                || (description.WorkflowId.ToLower()?.Contains(NameFilter.ToLower() ?? "") ?? false);
        }

        public bool MoveToTaskListFilter(FlowUXItemDescription description)
        {
            return PageNameFilter(description);
        }


        #endregion

        #region private methods

        private void ShowToken(string token)
        {
            D365AccessToken = token;
        }

        #endregion

        public void InitSolution(SolutionCollectoin solutionCollectoin)
        {
            if (solutionCollectoin != null && solutionCollectoin.value != null)
            {
                foreach (var item in solutionCollectoin.value.Where(p => p.uniquename.StartsWith("AIA") || p.uniquename.StartsWith("CLP")))
                {
                    SolutionItems.Add(new SearchDropdownItem { Name = item.uniquename, Value = item.solutionid, });
                }
                if (!string.IsNullOrEmpty(Settings.Current.D365SolutionId))
                {
                    CurrentSolutionDropdownItem = SolutionItems.FirstOrDefault(p => p.Value == Settings.Current.D365SolutionId);
                }
                else if (HistoryItems.Count > 0)
                {
                    CurrentSolutionDropdownItem = HistoryItems[0];
                }
            }
        }
    }
}
