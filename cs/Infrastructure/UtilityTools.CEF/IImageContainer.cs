using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UtilityTools.CEF
{
    public interface IImageContainer
    {
        IDictionary<string, byte[]> ImageSources { get; set; }
    }
}
