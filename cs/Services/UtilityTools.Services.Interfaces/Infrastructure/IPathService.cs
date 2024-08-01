using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure
{
    public interface IPathService
    {
        bool ClearTemperary();

        DirectoryInfo TemporaryLocation { get; }

        //DirectoryInfo MediaLocation { get; }

        DirectoryInfo ThumbnailLocation { get; }

        DirectoryInfo BookLocation { get; }

        DirectoryInfo CustomPngsLocation { get; }
        DirectoryInfo DefaultPngsLocation { get; }
        DirectoryInfo KeywordLocation { get; }
    }
}
