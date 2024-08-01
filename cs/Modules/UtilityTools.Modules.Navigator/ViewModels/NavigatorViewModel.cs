using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using UtilityTools.Core;
using UtilityTools.Core.Definition;
using UtilityTools.Core.Events;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Mvvm;
using UtilityTools.Modules.Navigator.ViewModels.Commands;

namespace UtilityTools.Modules.Navigator.ViewModels
{
    public class NavigatorViewModel : RegionViewModelBase
    {
        private string specificModuleName = "MediaGetTabs";
        private string activeWorkspace;
        private string previousMoudleName;

        public ObservableCollection<MenuItemDesription> MenuList { get; set; } = new ObservableCollection<MenuItemDesription>();

        public int menuItemIndex;
        public int MenuItemIndex
        {
            get { return menuItemIndex; }
            set
            {
                menuItemIndex = value;
            }
        }


        public string ActiveWorkspace
        {
            get { return activeWorkspace; }
            set
            {
                activeWorkspace = value;
            }
        }

        private readonly IContainerProvider containerProvider;
        private readonly IRegionManager regionManager;
        public ICommand MenuClick { get; set; }

        public NavigatorViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IContainerProvider containerProvider) : base(regionManager)
        {
            this.containerProvider = containerProvider;
            this.regionManager = regionManager;
            Initialize(regionManager);

            MenuClick = new MenuClickCommand(this);
            eventAggregator.GetEvent<ModuleEvent>().Subscribe(ControlModule);

            eventAggregator.GetEvent<AddExtendMenuEvent>().Subscribe(AddExtendMenu);

            eventAggregator.GetEvent<ModuleChangeEvent>().Subscribe(ChangeModule);
            eventAggregator.GetEvent<NavigateBackEvent>().Subscribe(NavigateBack);
        }


        private void AddExtendMenu()
        {
            MenuList.Add(new MenuItemDesription { Name = "Extend", ImageSource = MenuList.Last().ImageSource });
        }

        private void ControlModule(bool visiable)
        {
            MenuList.Single(p => p.Name == specificModuleName).Visiable = visiable;
        }

        private void ChangeModule(string moduleName)
        {
            var item = MenuList.SingleOrDefault(p => p.Name == moduleName);

            if (item != null)
            {
                MenuItemIndex = MenuList.IndexOf(item);
                ActivateView(moduleName);
            }
            else
            {
                MenuItemIndex = -1;
                ActivateView(moduleName);
            }
        }


        /// <summary>
        /// get the menu list
        /// </summary>
        /// <param name="regionManager"></param>
        private void Initialize(IRegionManager regionManager)
        {

            var views = regionManager.Regions[RegionNames.WorkspaceRegion].Views;

            foreach (var item in views)
            {
                var contentControl = item as ContentControl;

                if (contentControl.Resources["IsVisiable"] != null)
                {
                    var isVisiable = bool.Parse(contentControl.Resources["IsVisiable"] as string);
                    if (!isVisiable)
                    {
                        continue;
                    }
                }

                var imagePath = contentControl.Resources["MenuIcon"] as string;
                //var image = new BitmapImage();
                //image.BeginInit();
                //image.UriSource = new Uri("pack://application:,,,/UtilityTools.Modules.WebresouceManager;component/Resources/Images/Tank.png");
                //image.EndInit();

                var modeulName = contentControl.Resources["ModuelName"] as string;

                if (modeulName == specificModuleName && !Settings.Current.ShowMediaGetModule)
                {
                    MenuList.Add(new MenuItemDesription { Name = modeulName, ImageSource = $"pack://application:,,,/{item.GetType().Assembly.GetName().Name};component/{imagePath}", Visiable = false });
                }
                else
                {

                    MenuList.Add(new MenuItemDesription { Name = modeulName, ImageSource = $"pack://application:,,,/{item.GetType().Assembly.GetName().Name};component/{imagePath}" });
                }


            }


            if (Settings.Current.ShowMediaGetModule)
            {

                //ActivateView(ModuleName.MediaGet.ToString());
                ActivateView(ModuleName.MediaGetTabs.ToString());
            }
            else
            {

                ActivateView(ModuleName.WebResouce.ToString());
            }
        }

        public void NavigateBack()
        {
            ActivateView(previousMoudleName);
        }


        /// <summary>
        /// activate view
        /// </summary>
        /// <param name="viewName">specific name of view </param>
        public void ActivateView(string viewName)
        {
    
            // Deactivate current view
            IRegion workspaceRegion = regionManager.Regions[RegionNames.WorkspaceRegion];
            var views = workspaceRegion.Views;
            foreach (var view in views)
            {
                workspaceRegion.Deactivate(view);
            }

            // Activate named view
            var viewToActivate = regionManager.Regions[RegionNames.WorkspaceRegion].GetView(viewName);
            regionManager.Regions[RegionNames.WorkspaceRegion].Activate(viewToActivate);

            this.activeWorkspace = viewName;

            if (viewName != ModuleName.Browser.ToString())
            {
                this.previousMoudleName = viewName;
            }
        }

    }
}
