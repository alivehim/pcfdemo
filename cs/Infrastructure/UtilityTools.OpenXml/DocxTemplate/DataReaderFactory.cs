using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml.DocxTemplate
{
    internal class DataReaderFactory
    {
        public static DataReader CreateReader(XDocument dataDocument)
        {
            if (dataDocument.Root == null)
            {
                throw new Exception("root of data document is null");
            }

            DocToLowerCase(dataDocument);
            return new DataReader(dataDocument.Root);
        }

        private static void DocToLowerCase(XDocument doc)
        {
            foreach (var element in doc.Elements().DescendantsAndSelf())
            {
                element.Name = element.Name.ToString().ToLower();
            }

        }
    }
}
