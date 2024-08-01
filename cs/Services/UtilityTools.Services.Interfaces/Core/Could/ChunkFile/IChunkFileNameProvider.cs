using System.Text;

namespace UtilityTools.Services.Interfaces.Core.Could.ChunkFile
{
    public interface IChunkFileNameProvider
    {
        string GetIndividualChunkFileName(int uniqueIteration, int requiredPadding, StringBuilder streamingFilePathBuilder);
    }
}