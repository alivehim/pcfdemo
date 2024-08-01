using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class SolutionDefinition
    {
        public string solutionid { get; set; }

        public string uniquename { get; set; }
    }

    public class SolutionCollectoin
    {
        public IList<SolutionDefinition> value { get; set; }
    }
}
