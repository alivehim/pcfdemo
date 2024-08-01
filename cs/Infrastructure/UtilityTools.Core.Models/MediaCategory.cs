using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.Attributes;

namespace UtilityTools.Core.Models
{
    public enum MediaCategory
    {
        [MediaCategory("windows.png")]
        Local,
        [MediaCategory("magnet.png")]
        Magnet,
        [MediaCategory("english.png")]
        EnglishLearning,
        [MediaCategory("cloud.png")]
        XRes,
        [MediaCategory("cloud.png")]
        Source,
        [MediaCategory("post.png")]
        Post,
        [MediaCategory("Book.png")]
        Book,
        [MediaCategory("image.png")]
        Image,
    }
}
