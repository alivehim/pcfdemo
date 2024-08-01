using Prism.Regions;
using System.Windows.Controls;
using UtilityTools.Core;
using UtilityTools.Core.Definition;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Modules.Canvas.Views.Components;

namespace UtilityTools.Modules.Canvas.Views
{
    /// <summary>
    /// Interaction logic for CanvasView
    /// </summary>
    public partial class CanvasView : UserControl
    {
        public CanvasView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadRegion();
        }

        private void LoadRegion()
        {
            //https://prismlibrary.com/docs/wpf/view-composition.html
            //https://stackoverflow.com/questions/69661420/how-can-i-set-region-manager-for-my-dialog-window-in-prism
            var _regionManager = ToolsContext.Current.UnityContainer.ResolveService<IRegionManager>();
            RegionManager.SetRegionManager(cc, _regionManager);
            RegionManager.SetRegionName(cc, RegionNames.WidgetRegion);

            if (_regionManager.Regions[RegionNames.WidgetRegion].GetView(WidgetModuleName.XmlRegion.ToString()) == null)
            {
                _regionManager.Regions[RegionNames.WidgetRegion].Add(ToolsContext.Current.UnityContainer.ResolveService<AutomateFlowWidgetView>(), WidgetModuleName.AutomateFlowRegion.ToString());
                _regionManager.Regions[RegionNames.WidgetRegion].Add(ToolsContext.Current.UnityContainer.ResolveService<XmlWidgetView>(), WidgetModuleName.XmlRegion.ToString());
                _regionManager.Regions[RegionNames.WidgetRegion].Add(ToolsContext.Current.UnityContainer.ResolveService<DictionaryWidgetView>(), WidgetModuleName.DictionaryRegion.ToString());
                _regionManager.Regions[RegionNames.WidgetRegion].Add(ToolsContext.Current.UnityContainer.ResolveService<TexttoSpeechWidgetView>(), WidgetModuleName.TexttoSpeech.ToString());
                _regionManager.Regions[RegionNames.WidgetRegion].Add(ToolsContext.Current.UnityContainer.ResolveService<TextFormatWidgeView>(), WidgetModuleName.TextFormat.ToString());
                _regionManager.Regions[RegionNames.WidgetRegion].Add(ToolsContext.Current.UnityContainer.ResolveService<DocxWidgetView>(), WidgetModuleName.Docx.ToString());
                //_regionManager.Regions[RegionNames.WidgetRegion].Add(ToolsContext.Current.UnityContainer.ResolveService<FlowHistoryWidgeView>(), WidgetModuleName.FlowHistory.ToString());
            }
        }
    }
}
