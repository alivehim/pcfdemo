using SharpCompress.Archives;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UtilityTools.Core.Infrastructure;

namespace UtilityTools.Core.Utilites
{
    public class ZipHelper
    {
        public static string GetContentFromZip(string fullPath)
        {
            var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Read);
            {

                var archive = ArchiveFactory.Open(stream, new ReaderOptions
                {
                });

                var entries = archive.Entries.Where(e => !e.IsDirectory && e.Key.ToLower().EndsWith(".txt")).OrderBy(p => p.Key);

                var firstEntry = entries.ElementAt(0);

                var memoryStream = new MemoryStream();
                using (var entryStream = firstEntry.OpenEntryStream())
                {
                    entryStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    using (BinaryReader br = new BinaryReader(memoryStream))
                    {

                        var encoding = FileHelper.GetFileEncodeType(br, memoryStream.Length);
                        memoryStream.Position = 0;
                        using (StreamReader reader = new StreamReader(memoryStream, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }

            }
        }


        public static bool IsContainImage(string file)
        {
            using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (var archive = ArchiveFactory.Open(stream, new ReaderOptions
                {
                }))
                {
                    var entries = archive.Entries.Where(e => !e.IsDirectory && e.Key.ToLower().EndsWith(".jpg")).OrderBy(p => p.Key).ToList();
                    return entries.Count >= 2;
                }
            }
        }


        private static MemoryStream getMemoryStream(IArchiveEntry zipArchiveEntry)
        {
            
                var memoryStream = new MemoryStream();
                using (var entryStream = zipArchiveEntry.OpenEntryStream())
                {
                    entryStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }

                return memoryStream;
        }

        public static BitmapImage GetCoverFromZipFile(string file)
        {
            using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (var archive = ArchiveFactory.Open(stream, new ReaderOptions
                {
                }))
                {
                    var entries = archive.Entries.Where(e => !e.IsDirectory && e.Key.ToLower().EndsWith(".jpg")).OrderBy(p => p.Key);
                    if (entries.Count() != 0)
                    {
                        var picture = StreamToImage.GetImageFromStreamBug(getMemoryStream(entries.ElementAt(0)));

                        return picture;
                    }
                    return null;
                }
            }
        }

    }
}
