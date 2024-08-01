using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IPngImageCategoryRelationService
    {
        void Add(PngImageCategoryRelation model);
        void Delete(int pngImageId, int labelId);

        PngImageCategoryRelation FindById(int pngImageId, int labelId);

        PngImageCategoryRelation FindName(string lableName, string pngName);

        void Delete(PngImageCategoryRelation pngImageLabelRelation);

        void DeleteByLabel(int labelId);

        void ClearAll();
    }
}