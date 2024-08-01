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
using UtilityTools.Modules.WebresourceManager.ViewModels.Commands;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Modules.WebresourceManager.ViewModels
{
    [MessageOwner(MessageOwner.WebresourceManager)]
    public class WebResourceManagerViewModel : BaseModuleViewModel<FileUXItemDescription>
    {
        #region private variables
        //private string token;
        private string nameFilter;
        private WebResourceMetaDefinition webresouceMatadata;
        private SearchDropdownItem selectedSolutionDropdownItem = new SearchDropdownItem();

        private IList<string> fileChangeList = null;

        private FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

        #endregion

        public WebResourceManagerViewModel(IModuleMessageRepository moduleMessageRepository,
            IUXUpdateMessageRepository uXUpdateMessageRepository,
            ISearchHistoryService searchHistoryService,
            ITaskManager taskManager,
            IEventAggregator eventAggregator,
            IContainerProvider containerProvider
            ) :
            base(moduleMessageRepository, uXUpdateMessageRepository, searchHistoryService, taskManager, eventAggregator, containerProvider)
        {
            Load = new LoadCommand(this);
            FetchToken = new FetchTokenCommand(this);
            FetchSolutions = new FetchSolutionsCommand(this);
            ClearFilter = new ClearFilterCommand(this);
            ApppendToTaskList = new ApppendToTaskListCommand(this);
            BulkPublish = new BulkPublishCommand(this);
            ClearBulkTasks = new ClearBulkTasksCommand(this);
            DeleteBulkItem = new DeleteBulkItemCommand(this);
            FilterChangedFilesCommand = new FilterChangedFilesCommand(this);
            FilterLocalChangedFilesCommand = new FilterLocalChangedFilesCommand(this);
            MoveToBulkListCommand = new MoveToBulkListCommand(this);
            InitToken();

            InitMonitor();
            //var period = TimeSpan.FromSeconds(1);
            //var observable = Observable.Interval(period);
            //observable.Subscribe(i => Info(i.ToString()));
            eventAggregator.GetEvent<TokenEvent>().Subscribe(ShowToken);
        }

        #region Props

        public ICommand Load { get; set; }
        public ICommand FetchToken { get; set; }
        public ICommand FetchSolutions { get; set; }
        public ICommand ClearFilter { get; set; }
        public ICommand ApppendToTaskList { get; set; }
        public ICommand BulkPublish { get; set; }
        public ICommand ClearBulkTasks { get; set; }
        public ICommand DeleteBulkItem { get; set; }
        public ICommand FilterChangedFilesCommand { get; set; }

        public ICommand FilterLocalChangedFilesCommand { get; set; }
        public ICommand MoveToBulkListCommand { get; set; }

        public ObservableCollection<SearchDropdownItem> SolutionItems { get; set; } = new ObservableCollection<SearchDropdownItem>();

        public ObservableCollection<FileUXItemDescription> PublishingTaskList { get; set; } = new ObservableCollection<FileUXItemDescription>();

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

        //public string Token
        //{
        //    get
        //    {
        //        return token;
        //    }
        //    set
        //    {
        //        token = value;
        //        RaisePropertyChanged("Token");

        //    }
        //}

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


        public WebResourceMetaDefinition WebresouceMatadata
        {
            get
            {
                return webresouceMatadata;
            }
            set
            {
                webresouceMatadata = value;
                MatchWebresources();
            }
        }

        #endregion

        #region commands

        public ICommand OpenFolderCommand => new DelegateCommand((obj) =>
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c code ." )
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                WindowStyle = ProcessWindowStyle.Normal
            };

            //Create the output and streamreader to get the output
            //try the process
            try
            {
                Process process = new Process
                {
                    StartInfo = processStartInfo
                };

                processStartInfo.WorkingDirectory = Url;
                process.Start();

                process.WaitForExit();

                //get the output

                process.Close();
            }
            catch (Exception)
            {
            }
        });


        #endregion
        #region protected methods


        protected override void UpdateItems(IExtractResult<BaseResourceMetadata> data)
        {

            if (data is ExtractResult<FileDataDescriptor>)
            {
                var page = data as ExtractResult<FileDataDescriptor>;

                PageList.Clear();
                foreach (var item in page.Collection)
                {
                    try
                    {

                        var vItem = new FileUXItemDescription
                        {
                            FileName = item.FileName,
                            FullName = item.FullName,
                            MessageOwner = MessageOwner.WebresourceManager
                        };

                        PageList.Add(vItem);
                    }
                    catch (Exception ex)
                    {
                        Error(ex.ToString());
                    }
                }

                RefreshPageData();

                if (!string.IsNullOrEmpty(Settings.Current.D365AccessToken) && CurrentSolutionDropdownItem != null)
                {
                    //if (WebresouceMatadata == null)
                    {
                        IsWaiting = true;
                        var webresourceservice = ToolsContext.Current.UnityContainer.ResolveService<IWebresourceServers>();

                        Task.Run(async () =>
                        {
                            return await webresourceservice.GetWebresourcesAsync(Settings.Current.D365AccessToken);
                        }).ContinueWith((task) =>
                        {
                            if (task.Exception != null)
                            {
                                Error(task.Exception);
                            }
                            WebresouceMatadata = task.Result;
                            MatchWebresources();
                            IsWaiting = false;

                            MonitorDirectory(Url);
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    //else
                    //{
                    //    MatchWebresources();
                    //}

                }

                //var period = TimeSpan.FromSeconds(1);
                //var observable = Observable.Interval(period);
                //observable.Subscribe(i => PageList[0].MessageListData.Add(new ErrorListDataEntry { 

                //     Description ="abc",
                //       Level = ErrorListLevel.Information,
                //}));

            }

        }


        protected void InitToken()
        {
            //var settingService = ToolsContext.Current.UnityContainer.ResolveService<IUtilityToolsSettingService>();

            //var token = Settings.Current.D365AccessToken;

            //if (!string.IsNullOrEmpty(token))
            //{
            //    Token = token;
            //}
        }


        protected override bool PageNameFilter(FileUXItemDescription description)
        {
            if (string.IsNullOrEmpty(NameFilter) && fileChangeList == null)
                return true;

            if (fileChangeList != null)
            {
                if (!fileChangeList.Any(p => description.Name?.Contains(p) ?? false))
                {
                    return false;
                }
            }

            return description.Name?.Contains(NameFilter ?? "") ?? false;
        }

        public bool MoveToTaskListFilter(FileUXItemDescription description)
        {
            return PageNameFilter(description);
        }

        protected override void DoubleClickItem(BaseUXItemDescription baseUXItemDescription)
        {
            var fileDescription = baseUXItemDescription as FileUXItemDescription;
            if (!PublishingTaskList.Any(p => p == fileDescription))
            {
                PublishingTaskList.Add(fileDescription);
            }
        }

        #endregion

        #region private methods

        private void ShowToken(string token)
        {
            //Token = token;
        }
        private void MatchWebresources()
        {
            if (webresouceMatadata != null)
            {
                foreach (var item in PageList)
                {
                    if (IsWebResourceExists(item.FileName, out string objectid, out string displayname, out string name))
                    {
                        item.ObjectId = objectid;
                        item.DisplayName = displayname;
                        item.Name = name;
                    }
                }
            }
        }

        private bool IsWebResourceExists(string fileName, out string objectid, out string displayname, out string name)
        {
            objectid = string.Empty;
            displayname = string.Empty;
            name = string.Empty;

            var aiakeyname = $"aia_{Regex.Replace(fileName, "(.jpg|.js|.html|.css)", "")}";
            if (webresouceMatadata.value.Any(p => p.msdyn_name.Replace("aia_", "") == fileName || p.msdyn_name.Replace("clp_", "") == fileName))
            {
                var matadata = webresouceMatadata.value.Single(p => p.msdyn_name.Replace("aia_", "") == fileName || p.msdyn_name.Replace("clp_", "") == fileName);
                objectid = matadata.msdyn_objectid;
                displayname = matadata.msdyn_displayname;
                name = matadata.msdyn_name;
                return true;
            }
            else if (webresouceMatadata.value.Any(p => p.msdyn_name == aiakeyname))
            {
                var matadata = webresouceMatadata.value.Single(p => p.msdyn_name == aiakeyname);
                objectid = matadata.msdyn_objectid;
                displayname = matadata.msdyn_displayname;
                name = matadata.msdyn_name;
                return true;
            }


            return false;
        }

        //public void FilterChangedFiles(IList<string> files)
        //{
        //    fileChangeList = files;
        //}

        #endregion

        public void InitSolution(SolutionCollectoin solutionCollectoin)
        {
            if (solutionCollectoin != null && solutionCollectoin.value != null)
            {
                foreach (var item in solutionCollectoin.value/*.Where(p => p.uniquename.StartsWith("AIA") || p.uniquename.StartsWith("CLP"))*/)
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

        #region filewather


        private void InitMonitor()
        {
            fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes |
NotifyFilters.CreationTime |
NotifyFilters.LastWrite |
NotifyFilters.DirectoryName |
NotifyFilters.FileName |
NotifyFilters.Size;

            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;

        }
        private void MonitorDirectory(string path)
        {

            fileSystemWatcher.Path = path;
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {

            if (e.Name.Contains('\\'))
            {
                var pos = e.Name.LastIndexOf('\\') + 1;

                var name = e.Name.Substring(pos, e.Name.Length - pos);
                var description = PageList.FirstOrDefault(p => p.FileName == name);
                if (description != null)
                {
                    description.TaskStage = TaskStage.Prepared;
                }

            }
            else if (PageList.Any(p => p.FileName == e.Name))
            {
                var description = PageList.First(p => p.FileName == e.Name);
                description.TaskStage = TaskStage.Prepared;
            }

        }


        #endregion

    }
}
