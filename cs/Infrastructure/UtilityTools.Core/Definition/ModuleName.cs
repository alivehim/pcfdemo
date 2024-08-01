using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Definition
{
    public enum ModuleName
    {
        [Description("WebresouceManager")]
        WebResouce,
        EntityManager,
        MediaGet,
        MediaGetTabs,
        AppSettings,
        DataManager,
        Canvas,
        Extend,
        Browser,
        AutomateFlow,
        PowerPlatform,
        DataViewer
    }

    public enum MeidaModuleName
    {
        EmptyRegion,
        FileRightRegion,
        MediaControlPanel,
        MagnetPanelRegion,
        FolderRegion,
        MicrosoftLearnRegion,
        ImageControl
    }

    public enum WidgetModuleName
    {
        AutomateFlowRegion,
        XmlRegion,
        DictionaryRegion,
        TexttoSpeech,
        TextFormat,
        FlowHistory,
        Docx
    }
}
