using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Core
{
    public interface IAllInterviewService
    {
        Task<IList<string>> GetAnswersAsync(string url);
    }
}
