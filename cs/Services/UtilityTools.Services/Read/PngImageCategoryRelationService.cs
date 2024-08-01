using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Read;

namespace UtilityTools.Services.Read
{
    public class PngImageCategoryRelationService : IPngImageCategoryRelationService
    {
        private readonly IRepository<PngImageCategoryRelation> _PngImageLabelRelationRepository;

        public PngImageCategoryRelationService(IRepository<PngImageCategoryRelation> PngImageLabelRelationRepository)
        {
            _PngImageLabelRelationRepository = PngImageLabelRelationRepository;
        }

        public void Add(PngImageCategoryRelation model)
        {

            var states = _PngImageLabelRelationRepository.Attach<PngCategory>(model.PngCategory);
            if (model.PngImage.Id != 0)
            {
                var relation = _PngImageLabelRelationRepository.Table.FirstOrDefault(p => p.PngImageId == model.PngImage.Id && p.LabelId == model.PngCategory.Id);
                if (relation != null)
                    return;
                _PngImageLabelRelationRepository.Attach<PngImage>(model.PngImage);
            }
            _PngImageLabelRelationRepository.Insert(model);
        }

        public void Delete(int pngImageId, int labelId)
        {
            var model = _PngImageLabelRelationRepository.Table.FirstOrDefault(p => p.LabelId == labelId && p.PngImageId == pngImageId);
            if (model != null)
            {
                _PngImageLabelRelationRepository.Delete(model);
            }
        }


        public void DeleteByLabel(int labelId)
        {
            var list = _PngImageLabelRelationRepository.Table.Where(p => p.LabelId == labelId).ToList();
            foreach (var item in list)
            {
                _PngImageLabelRelationRepository.Delete(item);
            }
        }

        public PngImageCategoryRelation FindById(int pngImageId, int labelId)
        {
            return _PngImageLabelRelationRepository.Table.FirstOrDefault(p => p.LabelId == labelId && p.PngImageId == pngImageId);
        }

        public PngImageCategoryRelation FindName(string lableName, string pngName)
        {
            return _PngImageLabelRelationRepository.Table.FirstOrDefault(p => p.PngImage.Name == pngName && p.PngCategory.Name == lableName);
        }

        public void Delete(PngImageCategoryRelation pngImageLabelRelation)
        {
            _PngImageLabelRelationRepository.Delete(pngImageLabelRelation);
        }

        public void ClearAll()
        {
            var list = _PngImageLabelRelationRepository.Table.ToList();
            foreach (var item in list)
            {
                _PngImageLabelRelationRepository.Delete(item);
            }
        }
    }
}
