using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Core.Model.Attributes
{
    public class MediaTypeAttribute : Attribute
    {
        public MediaTypeAttribute(MediaCategory mediaCategory,string image="")
        {
            MediaCategory = mediaCategory;
            Image = image;
        }

        public MediaCategory MediaCategory { get; set; }

        public string Image { get; set; }
    }
}
