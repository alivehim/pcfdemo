using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.OpenXml.DocxTemplate.Core
{
    public class DocxPackageHelper
    {
        public static PackagePart GetDocumentPart(Package package)
        {
            PackageRelationship? relationship = package.GetRelationshipsByType(NamespaceConsts.DocumentRelationshipType).FirstOrDefault();
            Uri docUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relationship?.TargetUri);
            return package.GetPart(docUri);
        }

        public static PackagePart GetPart(Package package, Uri uri)
        {
            return package.GetPart(uri);
        }
    }
}
