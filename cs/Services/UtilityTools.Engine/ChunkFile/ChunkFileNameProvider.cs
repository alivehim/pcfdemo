using System.Text;
using UtilityTools.Services.Interfaces.Core.Could.ChunkFile;

namespace UtilityTools.Engine.ChunkFile
{
    public class ChunkFileNameProvider : IChunkFileNameProvider
    {
        private const string _outputStreamFileExtension = ".ts";
        public string GetIndividualChunkFileName( int uniqueIteration, int requiredPadding, StringBuilder streamingFilePathBuilder)
        {
            streamingFilePathBuilder.Clear();
            streamingFilePathBuilder.Append("chunk");
            streamingFilePathBuilder.Append('_');
            streamingFilePathBuilder.Append(uniqueIteration.ToString().PadLeft(requiredPadding, '0'));
            streamingFilePathBuilder.Append(_outputStreamFileExtension);

            return streamingFilePathBuilder.ToString();
        }
    }
}
