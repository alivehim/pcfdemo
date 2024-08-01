using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.MessageBus;

namespace UtilityTools.Core.Models.DataDescriptor
{
    public class ViolaRequestDescription : BaseResourceMetadata
    {
        public string formId { get; set; }
        public string wf_request_id { get; set; }

        public int aia_com_priority { get; set; }

        public int? aia_sys_leadtime_reminder { get; set; }

        //public int aia_sys_form_type { get; set; }
        //public string aia_eaa_formid { get; set; }

        //public string aia_name { get; set; }
    }
}
