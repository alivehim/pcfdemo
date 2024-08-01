using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class DependencyMetadataInfo
    {
        public string requiredcomponentobjectid { get; set; }
        public string requiredcomponentdisplayname { get; set; }
        public string requiredcomponenttypename { get; set; }
        public string dependentcomponentdisplayname { get; set; }
        public string dependentcomponentobjectid { get; set; }

        public string dependentcomponenttypename { get; set; }
        public string requiredcomponentname { get; set; }
        public string dependencyid { get; set; }

    }

    public class DependencyMetadataCollection
    {
        public IList<DependencyMetadataInfo> DependencyMetadataInfoCollection { get; set; }
    }

    public class DependencyMetadataResponse
    {
        public DependencyMetadataCollection DependencyMetadataCollection { get; set; }
    }
}
