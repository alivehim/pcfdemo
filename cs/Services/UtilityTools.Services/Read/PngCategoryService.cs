using Microsoft.EntityFrameworkCore;
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
    public class PngCategoryService : IPngCategoryService
    {
        private readonly IRepository<PngCategory> _lableRepository;

        public PngCategoryService(IRepository<PngCategory> lableRepository)
        {
            _lableRepository = lableRepository;
        }
        public PngCategory Add(string name)
        {
            {
                var PngCategory = _lableRepository.Table.FirstOrDefault(p => p.Name == name);
                if (PngCategory != null)
                    return PngCategory;
                else
                {
                    var newPngCategory = new PngCategory { Name = name };
                    _lableRepository.Insert(newPngCategory);
                    return newPngCategory;
                }

            }
        }

        public IList<PngCategory> GetList(bool refresh = false)
        {
            return _lableRepository.Table.Include(p => p.PngImageLables).ThenInclude(img => img.PngImage).ToList();
        }
        public async Task<IList<PngCategory>> GetListAsync(bool refresh = false)
        {
            return await _lableRepository.Table.Include(p => p.PngImageLables).ThenInclude(img => img.PngImage).ToListAsync();
        }
        public PngCategory FindByName(string name)
        {
            return _lableRepository.Table.FirstOrDefault(p => p.Name == name);
        }

        public PngCategory FindByNameIncludeImages(string name)
        {
            return _lableRepository.Table.Include(p => p.PngImageLables).ThenInclude(img => img.PngImage).FirstOrDefault(p => p.Name == name);
        }

        public void Delete(string name)
        {
            var lable = _lableRepository.Table.FirstOrDefault(p => p.Name == name);
            _lableRepository.Delete(lable);
        }
    }
}
