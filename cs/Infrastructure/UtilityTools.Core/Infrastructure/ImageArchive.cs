using SharpCompress.Archives;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Core.Infrastructure
{
    public class ImageArchive : BaseUXItemDescription
    {
        private string filePath;
        private List<MangaPicture> pictures;

        private int index;
        private int count;

        public int Count
        {
            get
            {
                return count;
            }
        }

        public ImageArchive(string filePath)
        {
            // TODO validate archive.
            // TODO Ignore non-pictures ?
            this.filePath = filePath;
        }

        public void Open(string file, bool isConver = false)
        {
            using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read))
            {
                pictures = new List<MangaPicture> { };

                var archive = ArchiveFactory.Open(stream, new ReaderOptions
                {
                });

                var entries = archive.Entries.Where(e => !e.IsDirectory && e.Key.ToLower().EndsWith(".jpg")).OrderBy(p => p.Key);

                foreach (var item in entries)
                {

                    var picture = new MangaPicture(item);
                    pictures.Add(picture);
                    if (isConver)
                    {
                        break;
                    }
                }

                count = pictures.Count;
                index = 0;

            }
        }


      

        public bool IsExistsNextPicture()
        {
            return index < count - 1;
        }

        public MangaPicture GetCurrentPicture()
        {
            return pictures[index];
        }

        public MangaPicture GetNextPicture()
        {
            if (index < (count - 1))
            {
                index++;
            }
            return GetCurrentPicture();
        }

        public MangaPicture GetPreviousPicture()
        {
            if (index > 0)
            {
                index--;
            }
            return GetCurrentPicture();
        }
    }
}
