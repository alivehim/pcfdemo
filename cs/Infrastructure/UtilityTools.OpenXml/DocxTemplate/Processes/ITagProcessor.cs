using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.OpenXml.DocxTemplate.Processes
{
    internal interface ITagProcessor
    {
        void SetDataReader(DataReader dataReader);
        void AddProcessor(ITagProcessor processor);

        void Process();
    }
}
