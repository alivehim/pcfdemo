using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Model.Attributes;

namespace UtilityTools.Core.Models
{
    public enum MediaSymbolType
    {
        None = 0,
        [MediaType(MediaCategory.Local, "folder.png")]
        LOCAL,
        XHA,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        FX,
        PPV,
        DomesticNineOne,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        TKTube,
        XHOpen,
        [MediaType(MediaCategory.Magnet, "magnet.png")]
        Sokan,
        [MediaType(MediaCategory.Magnet, "magnet.png")]
        Cili,
        [MediaType(MediaCategory.Magnet, "magnet.png")]
        Cilime,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        BestJAV,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Covid,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Javfc2,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Missav,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Xian,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Thumbzilla,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        FC2Hub,
        [MediaType(MediaCategory.EnglishLearning, "learning.png")]
        QSBDC,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Javbigo,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        HQPorner,
        Soushu,
        [MediaType(MediaCategory.Local, "folder.png")]
        Everything,
        [MediaType(MediaCategory.Local, "folder.png")]
        Folder,
        [MediaType(MediaCategory.Local, "folder.png")]
        File,
        [MediaType(MediaCategory.EnglishLearning, "learning.png")]
        AllInterview,
        [MediaType(MediaCategory.Source, "magnet.png")]
        TERK,
        [MediaType(MediaCategory.Post, "blog.png")]
        Carldesouza,
        [MediaType(MediaCategory.Post, "blog.png")]
        Inogic,
        [MediaType(MediaCategory.Post, "blog.png")]
        Sharepains,
        [MediaType(MediaCategory.Post, "blog.png")]
        Matthewdevaney,
        [MediaType(MediaCategory.Post, "blog.png")]
        Nebulaaitsolutions,
        [MediaType(MediaCategory.Post, "blog.png")]
        XRMtricks,
        [MediaType(MediaCategory.Book, "bookicon.png")]
        MicrosoftLearn,
        [MediaType(MediaCategory.Book, "bookicon.png")]
        MicrosoftTroubleshoot,
        [MediaType(MediaCategory.Book, "bookicon.png")]
        Openpress,
        [MediaType(MediaCategory.Book, "bookicon.png")]
        Opentextbc,
        [MediaType(MediaCategory.Post, "blog.png")]
        Dzone,
        [MediaType(MediaCategory.Post, "blog.png")]
        Mytrial365,
        [MediaType(MediaCategory.Post, "blog.png")]
        HCLTech,
        Eloquentjavascript,
        Rx,
        [MediaType(MediaCategory.Post, "blog.png")]
        kubernetes,
        [MediaType(MediaCategory.EnglishLearning, "learning.png")]
        RachelsEnglish,
        [MediaType(MediaCategory.EnglishLearning, "learning.png")]
        GuitarLesson,
        Geeksforgeeks,
        [MediaType(MediaCategory.EnglishLearning, "learning.png")]
        Simplifyingtheory,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        JAVDB,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        BlackX,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        BlackY,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Deeper,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Vixen,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Hussiepass,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Tiny4K,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        DontBreakeMe,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        BBCPie,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        XVideo,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Noodlemagazine,
        [MediaType(MediaCategory.XRes, "magnet.png")]
        Bangbros18teens,
        [MediaType(MediaCategory.EnglishLearning, "blog.png")]
        Youtube,
        [MediaType(MediaCategory.Image, "image.png")]
        ACGN,
        [MediaType(MediaCategory.Image, "image.png")]
        Jitujun,
        [MediaType(MediaCategory.Image, "image.png")]
        Tuiimg,
        [MediaType(MediaCategory.Image, "image.png")]
        Kaka234,
    }
}
