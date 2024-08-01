using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IPngImageService
    {
        void Add(PngImage pngImage);
        PngImage Add(string name);
        void Delete(PngImage pngImage);

        PngImage FindByName(string name);

        void Delete(string pngImageName);

        PngImage FindByNameEx(string name);

        void ClearAll();
    }
}