using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could.ChunkFile;

namespace UtilityTools.Engine.ChunkFile
{
    public class ChunkFileCombiner : IChunkFileCombiner
    {
        public ChunkFileCombiner()
        {

        }

        public IEnumerable<FileInfo> GetChunkFileInfos(string chunkFileDirectory)
        {
            DirectoryInfo streamFileDirectory = new DirectoryInfo(chunkFileDirectory);

            return streamFileDirectory.GetFiles();
        }

        public FileInfo CombineChunkFiles(IStreamUXItemDescription item, IEnumerable<FileInfo> chunkFiles, string outputFilePath, CancellationToken cancellationToken)
        {
            int iteration = 0;
            int numberOfChunkFiles = chunkFiles.Count();

            //IProgressData progressData = new ProgressData();
            foreach (FileInfo chunkFile in chunkFiles)
            {
                iteration++;

                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                using (FileStream fileStream = new FileStream(outputFilePath, FileMode.Append))
                {
                    byte[] chunkBytes = File.ReadAllBytes(chunkFile.FullName);

                    fileStream.Write(chunkBytes, 0, chunkBytes.Length);
                }

                //progressData.Status = string.Format("{0} Combined: {1}", iteration, chunkFile.FullName);

                decimal percentDone = ((decimal)((decimal)iteration / (decimal)numberOfChunkFiles) * (decimal)100);
            }

            return new FileInfo(outputFilePath);
        }
    }
}
