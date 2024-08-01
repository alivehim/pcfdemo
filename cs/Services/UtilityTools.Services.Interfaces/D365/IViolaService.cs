using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.D365;

namespace UtilityTools.Services.Interfaces.D365
{
    public interface IViolaService
    {

        Task<eaa_form_collection> GetEaaForms(string authorization);


        Task<RequestFormCollection> GetRequestFormsAsync(string authorization);

        Task UpdateSubmissDate(string formId, string authorization);
        Task MockTimeLineDate(string aia_wf_requestid, string authorization);
    }
}
