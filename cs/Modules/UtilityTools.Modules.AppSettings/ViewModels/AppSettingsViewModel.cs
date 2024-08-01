using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.Engine;
using UtilityTools.Core.Models.Magnet;
using UtilityTools.Core.Models.UX;
using UtilityTools.Core.Mvvm;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class AppSettingsViewModel : ViewModelBase
    {
        private readonly string Owner = "MoveFolder";

        private readonly string TTSMakerTransferFolderOwner = "TTSMakerTransferFolder";

        //private MediaSymbolViewModel mediaSymbolViewModel = new MediaSymbolViewModel();
        //private UserViewModel userViewModel = new UserViewModel();
        //private KeywordManageViewModel keywordManageViewModel = new KeywordManageViewModel();
        //private BookKeywordManageViewModel bookKeywordManageViewModel = new BookKeywordManageViewModel();
        private SearchDropdownItem selectedDropdownItem;
        private SearchDropdownItem selectedSeller = new SearchDropdownItem();


        private SearchDropdownItem selectedTTSMakerDropdownItem;
        protected readonly ISearchHistoryService searchHistoryService;

        private readonly IPathService pathService;
        private readonly ISellerHubService sellerHubService;
        private readonly IEventAggregator eventAggregator;

        public ObservableCollection<SearchDropdownItem> MoveFoldItems { get; set; } = new ObservableCollection<SearchDropdownItem>();
        public ObservableCollection<SearchDropdownItem> SellerHubItems { get; set; } = new ObservableCollection<SearchDropdownItem>();
        public ObservableCollection<SearchDropdownItem> TTSMakerTransferItems { get; set; } = new ObservableCollection<SearchDropdownItem>();

        public AppSettingsViewModel(ISearchHistoryService searchHistoryService, IPathService pathService, ISellerHubService sellerHubService, IEventAggregator eventAggregator)
        {
            this.searchHistoryService = searchHistoryService;
            this.pathService = pathService;
            this.sellerHubService = sellerHubService;
            this.eventAggregator = eventAggregator;

            InitMoveFolderData();
            LoadSellers();
            eventAggregator.GetEvent<UpateSellerEvent>().Subscribe(() =>
            {
                LoadSellers();
            });
        }

        #region  Commands

        public ICommand DeleteFolderCommand => new DelegateCommand((obj) =>
        {
            if (!string.IsNullOrEmpty(MovedTargetFolder))
            {
                if (searchHistoryService.Delete(Owner, MovedTargetFolder))
                {
                    var item = MoveFoldItems.FirstOrDefault(p => p.Value == MovedTargetFolder);
                    if (item != null)
                    {
                        MoveFoldItems.Remove(item);
                        MovedTargetFolder = MoveFoldItems.First(p => !string.IsNullOrEmpty(p.Value))?.Value;
                    }
                }
            }
        });

        public ICommand DeleteTTSMakerTransferFolderCommand => new DelegateCommand((obj) =>
        {
            if (!string.IsNullOrEmpty(MovedTargetFolder))
            {
                if (searchHistoryService.Delete(Owner, MovedTargetFolder))
                {
                    var item = TTSMakerTransferItems.FirstOrDefault(p => p.Value == MovedTargetFolder);
                    if (item != null)
                    {
                        TTSMakerTransferItems.Remove(item);
                        MovedTargetFolder = TTSMakerTransferItems.First(p => !string.IsNullOrEmpty(p.Value))?.Value;
                    }
                }
            }
        });


        public ICommand ClearCacheCommand => new DelegateCommand((obj) =>
        {
            MemoryCacheManager.Clear();
        });
        public ICommand OpenDataViewCommand => new DelegateCommand((obj) =>
        {
            ToolsContext.Current.PublishEvent<ModuleChangeEvent, string>("DataViewer");
        });


        public ICommand OpenTempFolderCommand => new DelegateCommand((obj) =>
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@$"{pathService.TemporaryLocation}")
            {
                UseShellExecute = true
            };
            p.Start();
        });

        #endregion

        public SearchDropdownItem CurrentMoveFoldItem
        {
            get
            {
                return this.selectedDropdownItem;
            }
            set
            {
                if (selectedDropdownItem != null && !this.selectedDropdownItem.Equals(value))
                {
                    this.selectedDropdownItem = value;

                    MovedTargetFolder = value.Value;


                    base.RaisePropertyChanged("CurrentMoveFoldItem");


                }
            }
        }

        public SearchDropdownItem CurrentSeller
        {
            get
            {
                return this.selectedSeller;
            }
            set
            {
                if (value != null && selectedSeller != null && !this.selectedSeller.Equals(value))
                {
                    this.selectedSeller = value;



                    base.RaisePropertyChanged("CurrentSeller");
                    MovedTargetFolder = value.Value;

                }
            }
        }

        public SearchDropdownItem CurrentTTSMakeTransferItem
        {
            get
            {
                return this.selectedTTSMakerDropdownItem;
            }
            set
            {
                if (selectedTTSMakerDropdownItem != null && !this.selectedTTSMakerDropdownItem.Equals(value))
                {
                    this.selectedTTSMakerDropdownItem = value;

                    TTSMakerTransferFolder = value.Value;


                    base.RaisePropertyChanged("CurrentTTSMakeTransferItem");


                }
            }
        }



        protected virtual void InitMoveFolderData()
        {

            this.selectedDropdownItem = new SearchDropdownItem();
            var searchList = searchHistoryService.GetList(Owner, 0, 100);
            if (searchList != null && searchList.Count > 0)
            {
                foreach (var item in searchList)
                {
                    MoveFoldItems.Add(new SearchDropdownItem
                    {
                        Name = item.Url,
                        Value = item.Url,
                        ModifyTime = item.LatestUsedTime,
                        Icon = new BitmapImage(new Uri($@"/UtilityTools.Modules.AppSettings;component/Resources/Images/setting.png", UriKind.Relative))
                    });
                }

                if (!string.IsNullOrEmpty(MovedTargetFolder))
                {
                    CurrentMoveFoldItem = MoveFoldItems.FirstOrDefault(p => p.Value == MovedTargetFolder);
                }
            }


        }

        private void LoadSellers()
        {
            SellerHubItems.Clear();

            var sellers = sellerHubService.GetAll();

            if (sellers != null && sellers.Count > 0)
            {
                foreach (var item in sellers)
                {
                    SellerHubItems.Add(new SearchDropdownItem
                    {
                        Name = item.SellerName,
                        Value = item.StoragePath,
                        Icon = new BitmapImage(new Uri($@"/UtilityTools.Modules.AppSettings;component/Resources/Images/setting.png", UriKind.Relative))
                    });
                }

            }
        }

        protected virtual void InitTTSMakerTransferData()
        {

            this.selectedDropdownItem = new SearchDropdownItem();
            var searchList = searchHistoryService.GetList(TTSMakerTransferFolderOwner, 0, 100);
            if (searchList != null && searchList.Count > 0)
            {
                foreach (var item in searchList)
                {
                    TTSMakerTransferItems.Add(new SearchDropdownItem
                    {
                        Name = item.Url,
                        Value = item.Url,
                        ModifyTime = item.LatestUsedTime,
                        Icon = new BitmapImage(new Uri($@"/UtilityTools.Modules.AppSettings;component/Resources/Images/setting.png", UriKind.Relative))
                    });
                }

                if (!string.IsNullOrEmpty(TTSMakerTransferFolder))
                {
                    CurrentMoveFoldItem = TTSMakerTransferItems.FirstOrDefault(p => p.Value == TTSMakerTransferFolder);
                }
            }
        }


        //public MediaSymbolViewModel MediaSymbolSettings
        //{
        //    get
        //    {
        //        return mediaSymbolViewModel;
        //    }
        //    set
        //    {
        //        mediaSymbolViewModel = value;
        //    }
        //}

        //public UserViewModel UserViewModel
        //{
        //    get
        //    {
        //        return userViewModel;
        //    }
        //    set
        //    {
        //        userViewModel = value;
        //    }
        //}

        //public KeywordManageViewModel KeywordManageViewModel
        //{
        //    get
        //    {
        //        return keywordManageViewModel;
        //    }
        //    set
        //    {
        //        keywordManageViewModel = value;
        //    }
        //}

        //public BookKeywordManageViewModel BookKeywordManageViewModel
        //{
        //    get
        //    {
        //        return bookKeywordManageViewModel;
        //    }
        //    set
        //    {
        //        bookKeywordManageViewModel = value;
        //    }
        //}


        private string d365AccessToken = Settings.Current.D365AccessToken;
        public string D365AccessToken
        {
            get { return d365AccessToken; }
            set
            {

                Settings.Current.D365AccessToken = value;
                Settings.Current.Save(nameof(Settings.Current.D365AccessToken));
                SetProperty(ref d365AccessToken, value);
            }
        }

        private string d365ResourceUrl = Settings.Current.D365ResourceUrl;
        public string D365ResourceUrl
        {
            get { return d365ResourceUrl; }
            set
            {

                Settings.Current.D365ResourceUrl = value;
                Settings.Current.Save(nameof(Settings.Current.D365ResourceUrl));
                SetProperty(ref d365ResourceUrl, value);
            }
        }

        //private string d365SolutionId = Settings.Current.D365SolutionId;
        //public string D365SolutionId
        //{
        //    get { return d365SolutionId; }
        //    set
        //    {

        //        Settings.Current.D365SolutionId = value;
        //        Settings.Current.Save(nameof(Settings.Current.D365SolutionId));
        //        SetProperty(ref d365SolutionId, value);
        //    }
        //}

        private string d365UserName = Settings.Current.D365UserName;
        public string D365UsrName
        {
            get { return d365UserName; }
            set
            {

                Settings.Current.D365UserName = value;
                Settings.Current.Save(nameof(Settings.Current.D365UserName));
                SetProperty(ref d365UserName, value);
            }
        }

        private string d365Password = Settings.Current.D365Password;
        public string D365Password
        {
            get { return d365Password; }
            set
            {

                Settings.Current.D365Password = value;
                Settings.Current.Save(nameof(Settings.Current.D365Password));
                SetProperty(ref d365Password, value);
            }
        }




        //private string sokankanUrl = Settings.Current.SokankanUrl;
        //public string SokankanUrl
        //{
        //    get { return sokankanUrl; }
        //    set
        //    {

        //        Settings.Current.SokankanUrl = value;
        //        Settings.Current.Save(nameof(Settings.Current.SokankanUrl));
        //        SetProperty(ref sokankanUrl, value);
        //    }
        //}


        private string downloadRPCAddress = Settings.Current.DownloadRPCAddress;
        public string DownloadRPCAddress
        {
            get { return downloadRPCAddress; }
            set
            {
                Settings.Current.DownloadRPCAddress = value;
                Settings.Current.Save(nameof(Settings.Current.DownloadRPCAddress));
                SetProperty(ref downloadRPCAddress, value);
            }
        }

        private string m3u8LocationRoot = Settings.Current.M3u8LocationRoot;
        public string M3u8LocationRoot
        {
            get { return m3u8LocationRoot; }
            set
            {

                Settings.Current.M3u8LocationRoot = value;
                Settings.Current.Save(nameof(Settings.Current.M3u8LocationRoot));
                SetProperty(ref m3u8LocationRoot, value);
            }
        }

        private string ffmepgLocation = Settings.Current.FFMPEGLocation;
        public string FFMPEGLocation
        {
            get { return ffmepgLocation; }
            set
            {

                Settings.Current.FFMPEGLocation = value;
                Settings.Current.Save(nameof(Settings.Current.FFMPEGLocation));
                SetProperty(ref ffmepgLocation, value);
            }
        }

        private string rpcAddress = Settings.Current.RPCAddress;
        public string RPCAddress
        {
            get { return rpcAddress; }
            set
            {

                Settings.Current.RPCAddress = value;
                Settings.Current.Save(nameof(Settings.Current.RPCAddress));
                SetProperty(ref rpcAddress, value);
            }
        }


        private bool showMediaGetModule = Settings.Current.ShowMediaGetModule;
        public bool ShowMediaGetModule
        {
            get { return showMediaGetModule; }
            set
            {

                Settings.Current.ShowMediaGetModule = value;
                Settings.Current.Save(nameof(Settings.Current.ShowMediaGetModule));


                var eventAggregator = ToolsContext.Current.UnityContainer.ResolveService<IEventAggregator>();
                eventAggregator.GetEvent<ModuleEvent>().Publish(value);

                SetProperty(ref showMediaGetModule, value);
            }
        }

        private bool displayNotification = Settings.Current.DisplayNotification;
        public bool DisplayNotification
        {

            get { return displayNotification; }
            set
            {

                Settings.Current.DisplayNotification = value;
                Settings.Current.Save(nameof(Settings.Current.DisplayNotification));
                SetProperty(ref displayNotification, value);
            }
        }

        //private string ciliUrl = Settings.Current.CiliUrl;
        //public string CiliUrl
        //{

        //    get { return ciliUrl; }
        //    set
        //    {

        //        Settings.Current.CiliUrl = value;
        //        Settings.Current.Save(nameof(Settings.Current.CiliUrl));
        //        SetProperty(ref ciliUrl, value);
        //    }
        //}

        //private string shoushuUrl = Settings.Current.ShoushuUrl;
        //public string ShoushuUrl
        //{

        //    get { return shoushuUrl; }
        //    set
        //    {

        //        Settings.Current.ShoushuUrl = value;
        //        Settings.Current.Save(nameof(Settings.Current.ShoushuUrl));
        //        SetProperty(ref shoushuUrl, value);
        //    }
        //}

        //private string novelFolder = Settings.Current.NovelFolder;
        //public string NovelFolder
        //{

        //    get { return novelFolder; }
        //    set
        //    {

        //        Settings.Current.NovelFolder = value;
        //        Settings.Current.Save(nameof(Settings.Current.NovelFolder));
        //        SetProperty(ref novelFolder, value);
        //    }
        //}

        public string CurrentSellerItem { get; set; }

        private string movedTargetFolder = Settings.Current.MovedTargetFolder;
        public string MovedTargetFolder
        {

            get { return movedTargetFolder; }
            set
            {

                if (searchHistoryService.Save(new SearchHistoryItem
                {
                    CreatedTime = DateTime.Now,
                    Owner = Owner,
                    Url = value,
                }) > 0)
                {
                    this.MoveFoldItems.Insert(0, new SearchDropdownItem { Name = value, Value = value, ModifyTime = DateTime.Now });
                }

                Settings.Current.MovedTargetFolder = value;
                Settings.Current.Save(nameof(Settings.Current.MovedTargetFolder));
                SetProperty(ref movedTargetFolder, value);
            }
        }

        private string ttsMakerTransferFolder = Settings.Current.TTSMakerTransferFolder;
        public string TTSMakerTransferFolder
        {

            get { return ttsMakerTransferFolder; }
            set
            {

                if (searchHistoryService.Save(new SearchHistoryItem
                {
                    CreatedTime = DateTime.Now,
                    Owner = Owner,
                    Url = value,
                }) > 0)
                {
                    this.TTSMakerTransferItems.Insert(0, new SearchDropdownItem { Name = value, Value = value, ModifyTime = DateTime.Now });
                }

                Settings.Current.TTSMakerTransferFolder = value;
                Settings.Current.Save(nameof(Settings.Current.TTSMakerTransferFolder));
                SetProperty(ref ttsMakerTransferFolder, value);
            }
        }

        private string plateAssembles = Settings.Current.PlateAssembles;
        public string PlateAssembles
        {

            get { return plateAssembles; }
            set
            {

                Settings.Current.PlateAssembles = value;
                Settings.Current.Save(nameof(Settings.Current.PlateAssembles));
                SetProperty(ref plateAssembles, value);
            }
        }



        private bool isSearchFolder = Settings.Current.IsSearchFolder;
        public bool IsSearchFolder
        {

            get { return isSearchFolder; }
            set
            {

                Settings.Current.IsSearchFolder = value;
                Settings.Current.Save(nameof(Settings.Current.IsSearchFolder));
                SetProperty(ref isSearchFolder, value);
            }
        }

        private string oneDriveFolder = Settings.Current.OneDriveFolder;
        public string OneDriveFolder
        {

            get { return oneDriveFolder; }
            set
            {

                Settings.Current.OneDriveFolder = value;
                Settings.Current.Save(nameof(Settings.Current.OneDriveFolder));
                SetProperty(ref oneDriveFolder, value);
            }
        }

        private string ttsFolder = Settings.Current.TTSFolder;
        public string TTSFolder
        {

            get { return ttsFolder; }
            set
            {

                Settings.Current.TTSFolder = value;
                Settings.Current.Save(nameof(Settings.Current.TTSFolder));
                SetProperty(ref ttsFolder, value);
            }
        }

        private string graphToken = Settings.Current.GraphToken;
        public string GraphToken
        {

            get { return graphToken; }
            set
            {

                Settings.Current.GraphToken = value;
                Settings.Current.Save(nameof(Settings.Current.GraphToken));
                SetProperty(ref graphToken, value);
            }
        }

        private string onenoteToken = Settings.Current.OnenoteToken;
        public string OnenoteToken
        {

            get { return onenoteToken; }
            set
            {

                Settings.Current.OnenoteToken = value;
                Settings.Current.Save(nameof(Settings.Current.OnenoteToken));
                SetProperty(ref onenoteToken, value);
            }
        }

        private string ttsMakerFolder = Settings.Current.TTSMakerFolder;
        public string TTSMakerFolder
        {

            get { return ttsMakerFolder; }
            set
            {

                Settings.Current.TTSMakerFolder = value;
                Settings.Current.Save(nameof(Settings.Current.TTSMakerFolder));
                SetProperty(ref ttsMakerFolder, value);
            }
        }


        private string mangaFolder = Settings.Current.MangaFolder;
        public string MangaFolder
        {

            get { return mangaFolder; }
            set
            {

                Settings.Current.MangaFolder = value;
                Settings.Current.Save(nameof(Settings.Current.MangaFolder));
                SetProperty(ref mangaFolder, value);
            }
        }

        private string environmentId = Settings.Current.EnvironmentId;


        public string EnvironmentId
        {

            get { return environmentId; }
            set
            {

                Settings.Current.EnvironmentId = value;
                Settings.Current.Save(nameof(Settings.Current.EnvironmentId));
                SetProperty(ref environmentId, value);
            }
        }


        private string flowToken = Settings.Current.FlowToken;
        public string FlowToken
        {

            get { return flowToken; }
            set
            {

                Settings.Current.FlowToken = value;
                Settings.Current.Save(nameof(Settings.Current.FlowToken));
                SetProperty(ref flowToken, value);
            }
        }

        private string removingText = Settings.Current.RemovingText;
        public string RemovingText
        {

            get { return removingText; }
            set
            {

                Settings.Current.RemovingText = value;
                Settings.Current.Save(nameof(Settings.Current.RemovingText));
                SetProperty(ref removingText, value);
            }
        }
        private string vlcPath = Settings.Current.VLCPath;
        public string VLCPath
        {

            get { return vlcPath; }
            set
            {

                Settings.Current.VLCPath = value;
                Settings.Current.Save(nameof(Settings.Current.VLCPath));
                SetProperty(ref vlcPath, value);
            }
        }
        private int magnetSearchType = Settings.Current.MagnetSearchType;
        public int MagnetSearchType
        {

            get { return magnetSearchType; }
            set
            {

                Settings.Current.MagnetSearchType = value;
                Settings.Current.Save(nameof(Settings.Current.MagnetSearchType));
                SetProperty(ref magnetSearchType, value);
            }
        }


        private bool deleteTempFolder = Settings.Current.DeleteTempFolder;
        public bool DeleteTempFolder
        {

            get { return deleteTempFolder; }
            set
            {

                Settings.Current.DeleteTempFolder = value;
                Settings.Current.Save(nameof(Settings.Current.DeleteTempFolder));
                SetProperty(ref deleteTempFolder, value);
            }
        }

        private bool isUsingVLC = Settings.Current.IsUsingVLC;
        public bool IsUsingVLC
        {

            get { return isUsingVLC; }
            set
            {

                Settings.Current.IsUsingVLC = value;
                Settings.Current.Save(nameof(Settings.Current.IsUsingVLC));
                SetProperty(ref isUsingVLC, value);
            }
        }

        private bool displayImageWhenHovering = Settings.Current.DisplayImageWhenHovering;
        public bool DisplayImageWhenHovering
        {

            get { return displayImageWhenHovering; }
            set
            {

                Settings.Current.DisplayImageWhenHovering = value;
                Settings.Current.Save(nameof(Settings.Current.DisplayImageWhenHovering));
                SetProperty(ref displayImageWhenHovering, value);
            }
        }


        private int randomCount = Settings.Current.RandomCount;
        public int RandomCount
        {

            get { return randomCount; }
            set
            {

                Settings.Current.RandomCount = value;
                Settings.Current.Save(nameof(Settings.Current.RandomCount));
                SetProperty(ref randomCount, value);
            }
        }
        private int ttsSelectCount = Settings.Current.TTSSelectCount;
        public int TTSSelectCount
        {

            get { return ttsSelectCount; }
            set
            {

                Settings.Current.TTSSelectCount = value;
                Settings.Current.Save(nameof(Settings.Current.TTSSelectCount));
                SetProperty(ref ttsSelectCount, value);
            }
        }
        private LivingStreamDownloadProvider downloadProvider = Settings.Current.DownloadProvider == 1 ? LivingStreamDownloadProvider.Local : (Settings.Current.DownloadProvider == 2 ? LivingStreamDownloadProvider.VLC : LivingStreamDownloadProvider.Fack);
        public LivingStreamDownloadProvider DownloadProvider
        {
            get
            {
                return downloadProvider;
            }
            set
            {
                Settings.Current.DownloadProvider = (int)value;
                Settings.Current.Save(nameof(Settings.Current.DownloadProvider));
                SetProperty(ref downloadProvider, value);
            }
        }
    }
}
