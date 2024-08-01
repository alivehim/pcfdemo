using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class OnenoteDescriptionNode
    {
        public OnenoteDescriptionNode(OnenoteSource searchSource, Type handler)
        {
            SearchSource = searchSource;
            Handler = handler;
        }

        public OnenoteSource SearchSource { get; set; }
        public Type Handler { get; set; }
    }
}
