using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Magnet
{
    public class MagnetDescriptionNode
    {
        public MagnetDescriptionNode(MagnetSearchSource searchSource, Type handler)
        {
            SearchSource = searchSource;
            Handler = handler;
        }

        public MagnetSearchSource SearchSource { get; set; }
        public Type Handler { get; set; }
    }
}
