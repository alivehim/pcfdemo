using Microsoft.Extensions.Options;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Parsers;

namespace UtilityTools.OpenXml.DocxTemplate
{
    public class DocxGenerator
    {
        private XDocument GetDocumentByStream(Stream stream)
        {
            if (stream == null)
            {
                throw new Exception("stream can not be null");
            }

            stream.Seek(0, System.IO.SeekOrigin.Begin);
            using (var reader = XmlReader.Create(stream))
            {
                var partXml = XDocument.Load(reader);
                return partXml;
            }
        }


        public void Save(string originalFile, string filePath)
        {
            using (var templateStream = new FileStream(originalFile, FileMode.Open))
            {
                using (var destinationStream = new FileStream(filePath, FileMode.Create))
                {

                }
            }
        }

        public Stream Generate(Stream templateStream, XDocument data)
        {
            var outputStream = new MemoryStream();

            var dataReader = DataReaderFactory.CreateReader(data);
            templateStream.Seek(0, SeekOrigin.Begin);


            templateStream.CopyTo(outputStream);

            var doc = GetDocumentByStream(outputStream);
            var documentParser = new DocumentParser(doc.Root);

            var processor = documentParser.Parse();

            processor.Process(dataReader);

            //outputStream.SetLength(0);
            //outputStream.Seek(0,System.IO.SeekOrigin.Begin);
            //using (var writer = new XmlTextWriter(outputStream, new UTF8Encoding()))
            //{
            //    doc.Save(writer);
            //}
            //return outputStream;

            var re = new MemoryStream();

            doc.Save(re);

            return re;

        }

        public XDocument GenerateXDocument(Stream templateStream, XDocument data)
        {
            var outputStream = new MemoryStream();

            var dataReader = DataReaderFactory.CreateReader(data);
            templateStream.Seek(0, SeekOrigin.Begin);


            templateStream.CopyTo(outputStream);

            var doc = GetDocumentByStream(outputStream);
            var documentParser = new DocumentParser(doc.Root);

            var processor = documentParser.Parse();

            processor.Process(dataReader);

            //outputStream.SetLength(0);
            //outputStream.Seek(0,System.IO.SeekOrigin.Begin);
            //using (var writer = new XmlTextWriter(outputStream, new UTF8Encoding()))
            //{
            //    doc.Save(writer);
            //}
            //return outputStream;

            return doc;
        }


    }
}
