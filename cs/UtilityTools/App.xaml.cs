using CefSharp;
using CefSharp.Handler;
using CefSharp.Wpf;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using UtilityTools.CEF;
using UtilityTools.CEF.Handlers;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.MessageBus;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.D365;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.Magnet;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Data;
using UtilityTools.Engine;
using UtilityTools.Engine.ChunkFile;
using UtilityTools.Engine.Download;
using UtilityTools.Extensions;
using UtilityTools.Interceptors;
using UtilityTools.Services;
using UtilityTools.Services.CloudService;
using UtilityTools.Services.CloudService.Vocabulary;
using UtilityTools.Services.Core;
using UtilityTools.Services.Core.Magnet;
using UtilityTools.Services.D365;
using UtilityTools.Services.Data;
using UtilityTools.Services.Google;
using UtilityTools.Services.Infrastructure;
using UtilityTools.Services.Infrastructure.MediaGet;
using UtilityTools.Services.Infrastructure.Queue;
using UtilityTools.Services.Infrastructure.TaskSchedule;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.CloudService;
using UtilityTools.Services.Interfaces.CloudService.Vocabulary;
using UtilityTools.Services.Interfaces.Core;
using UtilityTools.Services.Interfaces.Core.Could;
using UtilityTools.Services.Interfaces.Core.Could.ChunkFile;
using UtilityTools.Services.Interfaces.Core.Could.Download;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.Queue;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;
using UtilityTools.Services.Interfaces.Read;
using UtilityTools.Services.Interfaces.Vocabulary;
using UtilityTools.Services.Interfaces.Youtube;
using UtilityTools.Services.Read;
using UtilityTools.Services.Vocabulary;
using UtilityTools.Views;
using UtilityTools.Engine.Manga;
using UtilityTools.Core.Models.Engine;
using UtilityTools.Services.DataExtractDescriptors;
using UtilityTools.Services.DataExtractDescriptors.MediaGet;

namespace UtilityTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static string GetProjectPath()
        {
            string projectPath = Path.GetDirectoryName(typeof(App).Assembly.Location);
            string rootPath = Path.GetPathRoot(projectPath);
            while (Path.GetFileName(projectPath) != "UtilityTools")
            {
                if (projectPath == rootPath)
                    throw new DirectoryNotFoundException("Could not find the project directory.");
                projectPath = Path.GetDirectoryName(projectPath);
            }
            return projectPath;
        }

        private void InitCef3()
        {
            //string cefPath = Path.Combine(Path.GetDirectoryName(GetProjectPath()), "cef");
            //bool externalMessagePump = e.Args.Contains("--external-message-pump");

            //var settings = new CefSettings();
            //settings.MultiThreadedMessageLoop = !externalMessagePump;
            //settings.ExternalMessagePump = externalMessagePump;
            //settings.NoSandbox = true;
            //settings.WindowlessRenderingEnabled = true;
            //settings.LocalesDirPath = Path.Combine(cefPath, "Resources", "locales");
            //settings.ResourcesDirPath = Path.Combine(cefPath, "Resources");
            //settings.LogSeverity = CefLogSeverity.Warning;
            //settings.UncaughtExceptionStackSize = 8;

            const bool multiThreadedMessageLoop = true;

            IBrowserProcessHandler browserProcessHandler;

            if (multiThreadedMessageLoop)
            {
                browserProcessHandler = new CEF.Handlers.BrowserProcessHandler();
            }
            //else
            //{
            //    browserProcessHandler = new WpfBrowserProcessHandler(Dispatcher);
            //}

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = multiThreadedMessageLoop;
            settings.ExternalMessagePump = !multiThreadedMessageLoop;

            CefExample.Init(settings, browserProcessHandler: browserProcessHandler);

            //var settings = new CefSettings()
            //{
            //    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
            //    CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            //};

            ////Example of setting a command line argument
            ////Enables WebRTC
            //// - CEF Doesn't currently support permissions on a per browser basis see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access
            //// - CEF Doesn't currently support displaying a UI for media access permissions
            ////
            ////NOTE: WebRTC Device Id's aren't persisted as they are in Chrome see https://bitbucket.org/chromiumembedded/cef/issues/2064/persist-webrtc-deviceids-across-restart
            //settings.CefCommandLineArgs.Add("enable-media-stream");
            ////https://peter.sh/experiments/chromium-command-line-switches/#use-fake-ui-for-media-stream
            //settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
            ////For screen sharing add (see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access#comment-58677180)
            //settings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");

            ////Example of checking if a call to Cef.Initialize has already been made, we require this for
            ////our .Net 5.0 Single File Publish example, you don't typically need to perform this check
            ////if you call Cef.Initialze within your WPF App constructor.
            //if (!Cef.IsInitialized)
            //{
            //    //Perform dependency check to make sure all relevant resources are in our output directory.
            //    Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            //}

        }


        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //处理完后，我们需要将Handler=true表示已此异常已处理过
            e.Handled = true;

            if (!e.Exception.ToString().Contains("on an immutable object instance"))
            {
                ToolsContext.Current.PostMessage(e.Exception.ToString());
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    MessageBox.Show("应用程序发生异常" + exception.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出" + ex.ToString());
            }
        }

        protected override Window CreateShell()
        {
            ToolsContext.Current.ConfigureServices(Container);
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IShell, MainWindow>();
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();

            //containerRegistry.Register<AspectInterceptor>();

            //containerRegistry.Register<AsyncExceptionHandlingInterceptor>();

            //messagebus
            containerRegistry.RegisterSingleton<IMessageStreamProvider<IExtractResult<BaseResourceMetadata>>, MessageStreamProvider<IExtractResult<BaseResourceMetadata>>>();
            containerRegistry.RegisterSingleton<IModuleMessageRepository, ModuleMessageRepository>();

            containerRegistry.RegisterSingleton<IMessageStreamProvider<IBaseLogMetaData>, MessageStreamProvider<IBaseLogMetaData>>();
            containerRegistry.RegisterSingleton<ILogMessageRepository, LogMessageRepository>();

            containerRegistry.RegisterSingleton<IMessageStreamProvider<IUXMessage>, MessageStreamProvider<IUXMessage>>();
            containerRegistry.RegisterSingleton<IUXUpdateMessageRepository, UXMessageRepository>();


            containerRegistry.RegisterSingleton<ITaskManager, TaskManager>();
            containerRegistry.RegisterSingleton<IStreamFileExtractScheduler, StreamFileExtractScheduler>();

            containerRegistry.Register<IDataDescriptorFactory, DataDescriptorFactory>();
            containerRegistry.Register<IMediaPageDescriptorFactory, MediaPageDescriptorFactory>();
            containerRegistry.Register<StreamFileAnalyzerFactory>();

            containerRegistry.RegisterSingleton<IDataEngineFactory, DataEngineFactory>();
            containerRegistry.RegisterSingleton<ITaskEngine, TaskEngine>();

            containerRegistry.AddDescription<FileDataDescriptor, WebresourceExtractDescriptor>(MessageOwner.WebresourceManager);
            containerRegistry.AddDescription<EntityDescription, D365EntityExtractDescription>(MessageOwner.EntityManager);
            containerRegistry.AddDescription<FlowDescription, AutoflowExtractDescriptor>(MessageOwner.AutomateFlow);
            containerRegistry.AddDescription<MediaDataDescription, MediaGetExtractDescription>(MessageOwner.MediaGet);
            containerRegistry.AddDescription<ViolaRequestDescription, ViolaExtractDescription>(MessageOwner.DataManager);

            //Data
            containerRegistry.Register(typeof(IRepository<>), typeof(EFRepository<>));
            containerRegistry.Register<IDbContext, SqliteDbContext>();
            containerRegistry.Register<ISearchHistoryService, SearchHistoryService>();
            containerRegistry.Register<IMediaSymbolDBService, MediaSymbolDBService>();
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IUtilityToolsSettingService, UtilityToolsSettingService>();
            containerRegistry.Register<IGitService, GitService>();
            containerRegistry.Register<IMediaKeywordService, MediaKeywordService>();
            containerRegistry.Register<IMediaHistoryService, MediaHistoryService>();
            containerRegistry.Register<ISellerHubService, SellerHubService>();

            //Stream Engine
            containerRegistry.Register<IRPCDownloader, RPCDownloader>();

            containerRegistry.Register<IChunkFileProvider, ChunkFileProvider>();
            containerRegistry.Register<IChunkFileDownloader, ChunkFileDownloader>();
            containerRegistry.Register<IChunkFileCombiner, ChunkFileCombiner>();




            containerRegistry.Register<IImageDownloadEngine, ImageDownloadEngine>();
            containerRegistry.Register<IChunkFileNameProvider, ChunkFileNameProvider>();
            containerRegistry.Register<IDefaultChunkFileDownloader, ChunkFileMultiDownloader>();
            containerRegistry.Register<IPathService, PathService>();
            containerRegistry.RegisterSingleton<IDownloadProvider, DefaultDownloadProvider>();

            containerRegistry.RegisterSingleton<IDownloderSelector, DownloderSelector>();
            //Queue

            containerRegistry.RegisterSingleton(typeof(IBackgroundTaskQueue<>), typeof(BackgroundTaskQueue<>));
            containerRegistry.RegisterSingleton(typeof(ISimpleBackgroundTaskQueue<>), typeof(SimpleBackgroundTaskQueue<>));
            containerRegistry.RegisterSingleton(typeof(ITaskQueueResolver<>), typeof(TaskQueueResolver<>));
            containerRegistry.RegisterSingleton(typeof(ITaskQueueResolverAsync<>), typeof(TaskQueueResolverAsync<>));

            containerRegistry.RegisterSingleton<IAsyncTaskManager, AsyncTaskManager>();

            //D365
            //containerRegistry.Register<IWebresourceServers, WebresourceServers>().InterceptAsync<IWebresourceServers, AsyncExceptionHandlingInterceptor>();
            //containerRegistry.Register<IEntityDefinitionService, EntityDefinitionService>().InterceptAsync<IEntityDefinitionService, AsyncExceptionHandlingInterceptor>();

            containerRegistry.Register<AsyncInterceptor<UtilityToolInterceptor>>();
            containerRegistry.Register<IWebresourceServers, WebresourceServers>().InterceptAsync<IWebresourceServers, UtilityToolInterceptor>();
            containerRegistry.Register<IDynamicsService, DynamicsService>().InterceptAsync<IDynamicsService, UtilityToolInterceptor>();
            containerRegistry.Register<IEntityDefinitionService, EntityDefinitionService>().InterceptAsync<IEntityDefinitionService, UtilityToolInterceptor>();
            containerRegistry.Register<IViolaService, ViolaService>().InterceptAsync<IViolaService, UtilityToolInterceptor>();
            containerRegistry.Register<IFlowService, FlowService>().InterceptAsync<IFlowService, UtilityToolInterceptor>();

            containerRegistry.Register<IMicrosoftLearnService, MicrosoftLearnService>().InterceptAsync<IMicrosoftLearnService, UtilityToolInterceptor>();


            //MeidaGet
            containerRegistry.AddMediaDescription<MediaDataDescription, XHAPageDescriptor>(MediaSymbolType.XHA);
            containerRegistry.AddMediaDescription<MediaDataDescription, FXPageDescription>(MediaSymbolType.FX);
            containerRegistry.AddMediaDescription<MediaDataDescription, PPVPageDescriptor>(MediaSymbolType.PPV);
            containerRegistry.AddMediaDescription<MediaDataDescription, LocalFilesPageDescriptor>(MediaSymbolType.None);
            containerRegistry.AddMediaDescription<MediaDataDescription, Domestic91PageDescription>(MediaSymbolType.DomesticNineOne);
            containerRegistry.AddMediaDescription<MediaDataDescription, XianPageDescription>(MediaSymbolType.Xian);
            containerRegistry.AddMediaDescription<MediaDataDescription, ThumbzillaPageDescription>(MediaSymbolType.Thumbzilla);
            containerRegistry.AddMediaDescription<MediaDataDescription, XVideoDataDescription>(MediaSymbolType.XVideo);
            containerRegistry.AddMediaDescription<MediaDataDescription, NoodlemagazineDataDescription>(MediaSymbolType.Noodlemagazine);
            containerRegistry.AddMediaDescription<MediaDataDescription, TKTubePageDescription>(MediaSymbolType.TKTube);
            containerRegistry.AddMediaDescription<MediaDataDescription, XHOpenPageDescription>(MediaSymbolType.XHOpen);
            containerRegistry.AddMediaDescription<MediaDataDescription, SokanPageDescriptor>(MediaSymbolType.Sokan);
            containerRegistry.AddMediaDescription<MediaDataDescription, CiliPageDescriptor>(MediaSymbolType.Cili);
            containerRegistry.AddMediaDescription<MediaDataDescription, SoushuPageDescription>(MediaSymbolType.Soushu);
            containerRegistry.AddMediaDescription<MediaDataDescription, CilimePageDescriptor>(MediaSymbolType.Cilime);

            containerRegistry.AddMediaDescription<MediaDataDescription, BestJAVPageDescription>(MediaSymbolType.BestJAV);
            containerRegistry.AddMediaDescription<MediaDataDescription, CovidPageDescription>(MediaSymbolType.Covid);
            containerRegistry.AddMediaDescription<MediaDataDescription, Javfc2PageDescription>(MediaSymbolType.Javfc2);
            containerRegistry.AddMediaDescription<MediaDataDescription, HQPORNERPageDescription>(MediaSymbolType.HQPorner);
            containerRegistry.AddMediaDescription<MediaDataDescription, MissavPageDescription>(MediaSymbolType.Missav);
            containerRegistry.AddMediaDescription<MediaDataDescription, FC2HubPageDescription>(MediaSymbolType.FC2Hub);
            containerRegistry.AddMediaDescription<MediaDataDescription, JavbigoPageDescriptor>(MediaSymbolType.Javbigo);
            containerRegistry.AddMediaDescription<MediaDataDescription, JAVDBPageDescription>(MediaSymbolType.JAVDB);
            containerRegistry.AddMediaDescription<MediaDataDescription, BlackXPageDescription>(MediaSymbolType.BlackX);
            containerRegistry.AddMediaDescription<MediaDataDescription, BlackYPageDescription>(MediaSymbolType.BlackY);
            containerRegistry.AddMediaDescription<MediaDataDescription, DeeperPageDescription>(MediaSymbolType.Deeper);
            containerRegistry.AddMediaDescription<MediaDataDescription, DeeperPageDescription>(MediaSymbolType.Vixen);
            containerRegistry.AddMediaDescription<MediaDataDescription, HussiepassPageDescription>(MediaSymbolType.Hussiepass);
            containerRegistry.AddMediaDescription<MediaDataDescription, HussiepassPageDescription>(MediaSymbolType.XVideo);
            containerRegistry.AddMediaDescription<MediaDataDescription, Tiny4kPageDescription>(MediaSymbolType.Tiny4K);
            containerRegistry.AddMediaDescription<MediaDataDescription, DontBreakemePageDescription>(MediaSymbolType.DontBreakeMe);
            containerRegistry.AddMediaDescription<MediaDataDescription, BBCPiePageDescription>(MediaSymbolType.BBCPie);
            containerRegistry.AddMediaDescription<MediaDataDescription, Bangbros18teensPageDescription>(MediaSymbolType.Bangbros18teens);

            containerRegistry.AddMediaDescription<MediaDataDescription, EverythingPageDescrption>(MediaSymbolType.Everything);

            containerRegistry.AddMediaDescription<MediaDataDescription, QSBDCPageDescription>(MediaSymbolType.QSBDC);
            containerRegistry.AddMediaDescription<MediaDataDescription, AllInterviewPageDescription>(MediaSymbolType.AllInterview);
            containerRegistry.AddMediaDescription<MediaDataDescription, TERKPageDescription>(MediaSymbolType.TERK);
            containerRegistry.AddMediaDescription<MediaDataDescription, YoutubePageDescription>(MediaSymbolType.Youtube);

            containerRegistry.AddMediaDescription<MediaDataDescription, CarldesouzaPageDescription>(MediaSymbolType.Carldesouza);
            containerRegistry.AddMediaDescription<MediaDataDescription, InogicPageDescription>(MediaSymbolType.Inogic);
            containerRegistry.AddMediaDescription<MediaDataDescription, MatthewdevaneyPageDecription>(MediaSymbolType.Matthewdevaney);
            containerRegistry.AddMediaDescription<MediaDataDescription, SharepainsPageDescription>(MediaSymbolType.Sharepains);
            containerRegistry.AddMediaDescription<MediaDataDescription, NebulaaitsolutionsPageDescription>(MediaSymbolType.Nebulaaitsolutions);
            containerRegistry.AddMediaDescription<MediaDataDescription, XRMtricksDataDescription>(MediaSymbolType.XRMtricks);
            containerRegistry.AddMediaDescription<MediaDataDescription, MicrosoftLearnPageDescsription>(MediaSymbolType.MicrosoftLearn);
            containerRegistry.AddMediaDescription<MediaDataDescription, MicrosofTroubleShootPageDescription>(MediaSymbolType.MicrosoftTroubleshoot);
            containerRegistry.AddMediaDescription<MediaDataDescription, OpenpressPageDescription>(MediaSymbolType.Openpress);
            containerRegistry.AddMediaDescription<MediaDataDescription, OpentextbcPageDescription>(MediaSymbolType.Opentextbc);
            containerRegistry.AddMediaDescription<MediaDataDescription, DzonePageDescription>(MediaSymbolType.Dzone);
            containerRegistry.AddMediaDescription<MediaDataDescription, RachelsEnglishPageDescription>(MediaSymbolType.RachelsEnglish);
            containerRegistry.AddMediaDescription<MediaDataDescription, SimplifyingtheoryPageDescription>(MediaSymbolType.Simplifyingtheory);
            containerRegistry.AddMediaDescription<MediaDataDescription, ACGNPageDescription>(MediaSymbolType.ACGN);
            containerRegistry.AddMediaDescription<MediaDataDescription, GuitarLessonPageDescription>(MediaSymbolType.GuitarLesson);
            containerRegistry.AddMediaDescription<MediaDataDescription, GeeksforgeeksPageDescription>(MediaSymbolType.Geeksforgeeks);
            containerRegistry.AddMediaDescription<MediaDataDescription, Mytrial365PageDecription>(MediaSymbolType.Mytrial365);
            containerRegistry.AddMediaDescription<MediaDataDescription, HCLTechPageDescription>(MediaSymbolType.HCLTech);
            //containerRegistry.AddMediaDescription<MediaDataDescription, EloquentjavascriptPageDescription>(MediaSymbolType.Eloquentjavascript);
            containerRegistry.AddMediaDescription<MediaDataDescription, RXPageDescription>(MediaSymbolType.Rx);
            containerRegistry.AddMediaDescription<MediaDataDescription, kubernetesPageDescription>(MediaSymbolType.kubernetes);

            containerRegistry.AddMediaDescription<MediaDataDescription, JitujunPageDescription>(MediaSymbolType.Jitujun);
            containerRegistry.AddMediaDescription<MediaDataDescription, TuiimgPageDescription>(MediaSymbolType.Tuiimg);
            containerRegistry.AddMediaDescription<MediaDataDescription, Kaka234PageDescription>(MediaSymbolType.Kaka234);



            containerRegistry.Register<IMediaSymbolService, MediaSymbolService>();
            containerRegistry.Register<IContentCrawlingServiceFactory, ContentCrawlingServiceFactory>();
            containerRegistry.Register<IImageCollectorFactory, ImageCollectorFactory>();
            //containerRegistry.Register<IMagnetService, SokankanMagnetService>();

            //book

            containerRegistry.Register<IPngCategoryService, PngCategoryService>();
            containerRegistry.Register<IPngImageCategoryRelationService, PngImageCategoryRelationService>();
            containerRegistry.Register<IPngImageService, PngImageService>();


            containerRegistry.Register<IBookCategoryRelationService, BookCategoryRelationService>();
            containerRegistry.Register<IBookCategoryService, BookCategoryService>();
            containerRegistry.Register<IBookHistoryService, BookHistoryService>();
            containerRegistry.Register<IBookMarkService, BookMarkService>();
            containerRegistry.Register<IBookParseEngine, BookParseEngine>();
            containerRegistry.Register<IBookService, BookService>();

            //magnet

            containerRegistry.AddMagnetService<CiliMagnetService>(MagnetSearchSource.Cili);
            containerRegistry.AddMagnetService<SokankanMagnetService>(MagnetSearchSource.Sokankan);
            containerRegistry.AddMagnetService<CilimeMagentService>(MagnetSearchSource.Cilime);
            containerRegistry.Register<IMagnetServieFactory, MagnetServieFactory>();
            containerRegistry.Register<IJAVDBMagnetService, JAVDBMagnetService>();

            //downloadengine

            containerRegistry.Register<ILivingMediaDownloadEngineFactory, LivingMediaDownloadEngineFactory>();
            containerRegistry.AddDownloadEngineProvider<LivingSteamFileDownloadEngine>(LivingStreamDownloadProvider.Local);
            containerRegistry.AddDownloadEngineProvider<FakeLivingMediaDownloadEngine>(LivingStreamDownloadProvider.Fack);
            containerRegistry.AddDownloadEngineProvider<VLDMeidaDonwloadEngine>(LivingStreamDownloadProvider.VLC);

            containerRegistry.Register<IStreamFileDownloadEngine, StreamFileDownloadEngine>();
            //containerRegistry.Register<ISteamMediaDownloadEngine, FakeMediaDownloadEngine>();
            //onenote
            //containerRegistry.AddOnenoteService<GraphOnenoteService>(OnenoteSource.MicrosoftGraph);
            //containerRegistry.AddOnenoteService<OnenoteService>(OnenoteSource.Onenote);

            containerRegistry.Register<IGraphOnenoteService, GraphOnenoteService>().InterceptAsync<IGraphOnenoteService, UtilityToolInterceptor>(); ;
            //containerRegistry.RegisterInstance(new OnenoteDescriptionNode(OnenoteSource.MicrosoftGraph, typeof(GraphOnenoteService)));

            //containerRegistry.Register<IGraphOnenoteService, OnenoteService>().InterceptAsync<IGraphOnenoteService, UtilityToolInterceptor>(); ;
            //containerRegistry.RegisterInstance(new OnenoteDescriptionNode(OnenoteSource.Onenote, typeof(OnenoteService)));

            containerRegistry.Register<IOnenoteServiceFactory, OnenoteServiceFactory>();


            //others

            containerRegistry.Register<IPlateDetailService, PlateDetailService>();
            containerRegistry.Register<IAllInterviewService, AllInterviewService>();


            containerRegistry.Register<IVocabularyService, VocabularyService>();
            containerRegistry.Register<IDictionaryService, DictionaryService>();

            containerRegistry.Register<ICloudResourceService, CloudResourceService>();
            containerRegistry.Register<IYoutubeService, YoutubeService>();



            //cloud 
            containerRegistry.Register<IMangaDownloader, MangaDownloader>();
            containerRegistry.Register<IHttpService, HttpService>();



        }

        protected override void InitializeModules()
        {
            //var splashScreen = new SplashScreenWindow();
            //splashScreen.Show();
            //try
            //{
            //    base.InitializeModules();
            //}
            //finally
            //{
            //    splashScreen.Close();
            //}

            base.InitializeModules();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {

            InitCef3();
            /* Note that each module in this application has a post-build

             * event that copies the module to a 'Modules' subfolder in the 
             * output folder. Prism will use the DirectoryModuleCatalog that 
             * we create here to scan that folder to populate the catalog. */

            // Create a new module catalog and pass it to Prism
            var catalog = new DirectoryModuleCatalog();
            catalog.ModulePath = @".\Modules";

            return catalog;
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewName = viewType.FullName;
                viewName = $"{viewName.Replace(".Views.", ".ViewModels.")}";
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
                var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}{1}", viewName, suffix);

                var assembly = viewType.GetTypeInfo().Assembly;
                var type = assembly.GetType(viewModelName, true);

                return type;
            });
        }
    }
}
