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
    public class PngImageService : IPngImageService
    {
        private readonly IRepository<PngImage> _pngImageRepository;

        public PngImageService(IRepository<PngImage> pngImageRepository)
        {
            _pngImageRepository = pngImageRepository; ;
        }

        public PngImage Add(string name)
        {
            var model = _pngImageRepository.Table.FirstOrDefault(p => p.Name == name);
            if (model != null)
            {
                return model;
            }
            else
            {
                var newmodel = new PngImage { Name = name, Location = string.Empty };
                _pngImageRepository.Insert(newmodel);
                return newmodel;
            }
        }

        public void Add(PngImage pngImage)
        {
            _pngImageRepository.Insert(pngImage);
        }


        public void Delete(PngImage pngImage)
        {
            _pngImageRepository.Delete(pngImage);
        }

        public void Delete(string pngImageName)
        {
            var Model = _pngImageRepository.Table.FirstOrDefault(p => p.Name == pngImageName);

            _pngImageRepository.Delete(Model);
        }

        public PngImage FindByName(string name)
        {
            return _pngImageRepository.Table.FirstOrDefault(p => p.Name == name);
        }

        public PngImage FindByNameEx(string name)
        {
            return _pngImageRepository.Table.Include(p => p.PngImageLables).FirstOrDefault(p => p.Name == name);
        }

        public void ClearAll()
        {
            var list = _pngImageRepository.Table.ToList();
            foreach(var item in list)
            {
                _pngImageRepository.Delete(item);
            }
        }
    }
}
