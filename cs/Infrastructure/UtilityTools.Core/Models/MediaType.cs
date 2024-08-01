using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Attributes;

namespace UtilityTools.Core.Models
{
    public enum MediaType
    {
        None,
        [ButtionDisplay("mock", "copyname")]
        FX,
        [ButtionDisplay("mockppv", "relate", "copyname", "everything")]
        PPV,
        [ButtionDisplay("delete", "tag", "relate","copyname", "move2classfolder", "move2folder", "findfc2", "renamefc2", "mock", "loadImage")]
        File,
        [ButtionDisplay("downloadimage", "copyname", "everything", "imageQueue")]
        Image,
        [ButtionDisplay("delete", "copyname")]
        Audio,
        [ButtionDisplay("delete", "copyname")]
        PDF,
        [ButtionDisplay("delete","vedioQueue", "copyname", "getMediaSource", "everything", "openBrowser")]
        StreamMeida,
        [ButtionDisplay("getstreamlink", "getStreamLink", "copyname", "openurl", "getPreviewImages")]
        CloudStreamMeida,
        [ButtionDisplay("download", "delete", "copyname", "everything")]
        HandlingStreamMeida,
        [ButtionDisplay("copyname", "move2classfolder", "renamebook", "linkbookpanel")]
        Novel,
        [ButtionDisplay("delete", "tag", "move2folder", "mock", "copyname", "everything", "history", "loadImage")]
        Magnet,
        [ButtionDisplay("retrive", "copyname")]
        MagnetSearch,
        [ButtionDisplay("read")]
        OnlineResource,
        [ButtionDisplay("plate","openurl")]
        Plate,
        [ButtionDisplay( "zipfolder", "mock", "copyname")]
        Folder,
        [ButtionDisplay("openurl", "writeOnenote")]
        Post,
        [ButtionDisplay("openurl", "writeOnenote", "downsub", "tuberipper")]
        Rachels,
        [ButtionDisplay("openurl", "downloadmanga", "getHtmlSource")]
        ACGN,
        [ButtionDisplay("javdbsource","mock", "copyname", "everything", "move2folder", "history", "loadImage")]
        JAVDBMagnet,
        [ButtionDisplay("copyname", "openurl", "checkLocal", "downsub", "tuberipper")]
        Youtube,
    }
}
