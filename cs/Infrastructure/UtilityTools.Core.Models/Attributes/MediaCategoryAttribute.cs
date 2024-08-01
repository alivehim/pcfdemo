using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Attributes
{
    public class MediaCategoryAttribute : Attribute
    {
        public MediaCategoryAttribute(string image)
        {
            Image = image;
        }

        public string Image { get; set; }
    }
}
