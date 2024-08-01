using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Utilites;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Core.Infrastructure
{
    public class Settings
    {
        private static Settings current;
        public static Settings Current
        {
            get
            {
                lock (lockable)
                {
                    if (current == null)
                    {
                        current = Load();
                        SettingsChanged += Settings_SettingsChanged;
                    }
                    return current;
                }
            }
        }

        private static void Settings_SettingsChanged(object sender, EventArgs e)
        {
            lock (lockable)
            {
                current = Load();
            }
        }

        private static Settings Load()
        {
            try
            {
                var service = ToolsContext.Current.UnityContainer.ResolveService<IUtilityToolsSettingService>();
                var settings = service.GetAll();

                if (settings.ReadGroupSettings(out Settings s))
                {
                    return s;
                }
                else
                {
                    return new Settings();
                }
            }
            catch (Exception)
            {
                return new Settings();
            }
        }

        public void Save()
        {
            lock (lockable)
            {
                //LocalSettingsHelper.GetContainer(SETTINGS_CONTAINER).WriteGroupSettings(this);
                //SettingsChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public void Save(string key)
        {
            lock (lockable)
            {
                var type = typeof(Settings);
                var properties = type.GetProperties();
                var prop = properties.Single(p => p.Name == key);

                var service = ToolsContext.Current.UnityContainer.ResolveService<IUtilityToolsSettingService>();
                service.Save(key, prop.GetValue(this, null));
            }
        }


        public static event EventHandler SettingsChanged;

        private static object lockable = new object();


        public string D365AccessToken { get; set; }
        //public string SokankanUrl { get; set; }
        public string DownloadRPCAddress { get; set; }
        public string M3u8LocationRoot { get; set; }
        public string FFMPEGLocation { get; set; }
        public string RPCAddress { get; set; }
        public bool ShowMediaGetModule { get; set; }
        public bool DisplayNotification { get; set; }
        public bool LargeFontEnable { get; set; } = false;
        public string CiliUrl { get; set; }

        //public string ShoushuUrl { get; set; }
        public string TTSMakerFolder { get; set; }
        public string MangaFolder { get; set; }
        public string EnvironmentId { get; set; }
        public string FlowToken { get; set; }
        public string MovedTargetFolder { get; set; }
        public string TTSMakerTransferFolder { get; set; }
        public string PlateAssembles { get; set; }
        public string RemovingText { get; set; }
        public string VLCPath { get; set; }
        public int MagnetSearchType { get; set; }
        public int DownloadProvider { get; set; }
        public int RandomCount { get; set; }
        public bool DeleteTempFolder { get; set; }
        public bool IsUsingVLC { get; set; }
        public bool DisplayImageWhenHovering { get; set; }

        public bool IsSearchFolder { get; set; }
        public string OneDriveFolder { get; set; }
        public string TTSFolder { get; set; }
        public string GraphToken { get; set; }
        public string OnenoteToken { get; set; }


        public string D365ResourceUrl { get; set; }

        private string d365SolutionId = string.Empty;
        public string D365SolutionId {
            get { return d365SolutionId; }
            set
            {
                d365SolutionId = value;
                Save(nameof(Settings.Current.D365SolutionId));
            }
        }

        public string D365UserName { get; set; }
        public string D365Password { get; set; }

        public int TTSSelectCount { get; set; }


        //private string d365AccessToken = string.Empty;
        //public string D365AccessToken
        //{
        //    get { return d365AccessToken; }
        //    set
        //    {

        //        d365AccessToken = value;
        //        Save();
        //    }
        //}

        //private string sokankanUrl = Settings.Current.SokankanUrl;
        //public string SokankanUrl
        //{
        //    get { return sokankanUrl; }
        //    set
        //    {

        //        SokankanUrl = value;
        //        Save();
        //    }
        //}


        //private string downloadRPCAddress = string.Empty;
        //public string DownloadRPCAddress
        //{
        //    get { return downloadRPCAddress; }
        //    set
        //    {

        //        downloadRPCAddress = value;
        //        Save();
        //    }
        //}

        //private string m3u8LocationRoot = string.Empty;
        //public string M3u8LocationRoot
        //{
        //    get { return m3u8LocationRoot; }
        //    set
        //    {

        //        m3u8LocationRoot = value;
        //        Save();
        //    }
        //}

        //private string ffmepgLocation = string.Empty;
        //public string FFMPEGLocation
        //{
        //    get { return ffmepgLocation; }
        //    set
        //    {

        //        ffmepgLocation = value;
        //        Save();
        //    }
        //}

        //private bool showMediaGetModule = false;
        //public bool ShowMediaGetModule
        //{
        //    get { return showMediaGetModule; }
        //    set
        //    {
        //        showMediaGetModule = value;
        //        Save();
        //    }
        //}

        //private bool displayNotification = false;
        //public bool DisplayNotification
        //{

        //    get { return displayNotification; }
        //    set
        //    {

        //        displayNotification = value;
        //        Save();
        //    }
        //}

    }
}
