using System.Collections.Generic;
using System.Threading.Tasks;
using UtilityTools.Core.Models.Read;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IBookParseEngine
    {
        BookInfo CurrentBook { get; }
        string FileName { get; }

        Task Load(string fullName, IList<ProfileImage> keywordList, int width, int height, int lineHeight, int fontsize);
    }
}