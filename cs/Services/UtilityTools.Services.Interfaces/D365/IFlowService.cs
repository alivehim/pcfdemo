using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.D365;

namespace UtilityTools.Services.Interfaces.D365
{
    public interface IFlowService
    {
        Task<FlowHistoryCollection> GetFlowHistoryAsync(string environmentid, string flowid, string token="");

        Task<FlowHistoryCollection> GetFlowHistoryAsync(string nextUrl, string token = "");
        Task<TriggerDetail> GetFlowTriggerOutDetail(string url);
    }
}
