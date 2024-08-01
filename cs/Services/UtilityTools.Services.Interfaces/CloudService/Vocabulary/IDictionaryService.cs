using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.CloudService.Vocabulary
{
    public interface IDictionaryService
    {
        Task<string> GetBasicConceptAsync(string vocabulary);

        Task<string> GetExampleAsync(string vocabulary);

        Task<string> GetWordRootAsync(string vocabulary);

        Task<string> GetVariantsAsync(string vocabulary);

        Task<string> GetPhraseAsync(string vocabulary);
    }
}
