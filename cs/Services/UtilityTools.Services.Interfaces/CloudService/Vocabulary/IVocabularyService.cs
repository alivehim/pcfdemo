using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Vocabulary
{
    public interface IVocabularyService
    {
        Task<string> GetContent(string url);
    }
}
