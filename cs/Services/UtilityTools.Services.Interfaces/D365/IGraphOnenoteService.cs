using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.D365;

namespace UtilityTools.Services.Interfaces.D365
{
    public interface IGraphOnenoteService
    {
        Task<IList<BookItem>> GetOnenoteBooksAsync();

        Task<IList<SectionItem>> GetOnenoteSectionsAsync(string url);

        Task<PageResponse> GetOnenotePagesAsync(string url);

        Task<string> GetOnenoteContentAsync(string url);

        Task<SectionItem> CreateSectionAsync(string url, string sectionName);

        Task CreatePageAsync(string sectionUrl, string title, string content);
    }
}
