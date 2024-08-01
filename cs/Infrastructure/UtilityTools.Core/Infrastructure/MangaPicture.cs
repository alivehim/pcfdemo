using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UtilityTools.Core.Utilites;

namespace UtilityTools.Core.Infrastructure
{
    public class MangaPicture
    {
        private IArchiveEntry zipArchiveEntry;
        public string Name
        {
            get
            {
                return zipArchiveEntry.Key;
            }
        }

        private MemoryStream MemoryStream
        {
            get
            {
                //if (memoryStream == null)
                //{

                var memoryStream = new MemoryStream();
                using (var entryStream = zipArchiveEntry.OpenEntryStream())
                {
                    entryStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }

                //}
                //memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
        }

        public BitmapImage BitmapImage=>
             StreamToImage.GetImageFromStreamBug(MemoryStream, 0);

        public Image Image=> Image.FromStream(MemoryStream);

        public MangaPicture(IArchiveEntry entry)
        {
            zipArchiveEntry = entry;
        }
    }
}
