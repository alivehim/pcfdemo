using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.DataExtractDescriptors
{
    [MessageOwner(MessageOwner.DataManager)]
    public class ViolaExtractDescription : BaseSimpleExtractDescriptor<ViolaRequestDescription>
    {
        private readonly IViolaService violaService;

        public ViolaExtractDescription(IViolaService violaService)
        {
            this.violaService = violaService;
        }

        //public async override Task<IExtractResult<ViolaRequestDescription>> RunAsync(ITaskContext taskContext)
        //{
        //    var data = new List<ViolaRequestDescription>();

        //    var list = await violaService.GetRequestFormsAsync(taskContext.Key);

        //    foreach (var item in list.value)
        //    {
        //        data.Add(new ViolaRequestDescription
        //        {
        //            Name = item.aia_name,
        //            formId = item.aia_apv_request_formid,
        //            wf_request_id = item._aia_sys_wf_request_value,
        //            aia_com_priority = item.aia_com_priority,
        //            aia_sys_leadtime_reminder = item.aia_sys_leadtime_reminder

        //        }) ;
        //    }

        //    //var list = violaService.GetEaaForms(taskContext.Key).GetAwaiter().GetResult();

        //    //foreach (var item in list.value)
        //    //{
        //    //    data.Add(new ViolaRequestDescription
        //    //    {
        //    //        Name = item.aia_name,
        //    //        aia_eaa_formid = item.aia_eaa_formid,
        //    //        aia_sys_form_type = item.aia_sys_form_type,
        //    //    });
        //    //}

        //    return Result(data);
        //}

        public async override Task RunAsync(ITaskContext taskContext)
        {

            var data = new List<ViolaRequestDescription>();

            var list = await violaService.GetRequestFormsAsync(taskContext.Key);

            foreach (var item in list.value)
            {
                data.Add(new ViolaRequestDescription
                {
                    Name = item.aia_name,
                    formId = item.aia_apv_request_formid,
                    wf_request_id = item._aia_sys_wf_request_value,
                    aia_com_priority = item.aia_com_priority,
                    aia_sys_leadtime_reminder = item.aia_sys_leadtime_reminder

                });
            }

            //var list = violaService.GetEaaForms(taskContext.Key).GetAwaiter().GetResult();

            //foreach (var item in list.value)
            //{
            //    data.Add(new ViolaRequestDescription
            //    {
            //        Name = item.aia_name,
            //        aia_eaa_formid = item.aia_eaa_formid,
            //        aia_sys_form_type = item.aia_sys_form_type,
            //    });
            //}

            Result(data);
        }
    }
}
