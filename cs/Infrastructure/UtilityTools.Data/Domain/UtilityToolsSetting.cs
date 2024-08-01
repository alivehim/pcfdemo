using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class UtilityToolsSetting : BaseEntity
    {
        //public string D365AccessToken { get; set; }
        //public string SokankanUrl { get; set; }
        //public string DownloadRPCAddress { get; set; }
        //public string M3u8LocationRoot { get; set; }
        //public string FFMPEGLocation { get; set; }
        //public bool ShowMediaGetModule { get; set; }
        //public bool DisplayNotification { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
