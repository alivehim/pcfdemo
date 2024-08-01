using Prism.Commands;
using Prism.Mvvm;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using UtilityTools.OpenXml;
using UtilityTools.OpenXml.DocxTemplate;
using UtilityTools.OpenXml.DocxTemplate.Core;

namespace UtilityTools.Modules.Canvas.ViewModels
{
    public class DocxWidgetViewModel : BindableBase
    {
        private DocxReader docxReader;
        private string docxContent;
        public string DocxContent
        {
            get
            {
                return docxContent;
            }
            set
            {
                docxContent = value;
                RaisePropertyChanged("DocxContent");
            }
        }

        private FlowDocument flowDocument;
        public FlowDocument FlowDocument
        {
            get
            {
                return flowDocument;
            }
            set
            {
                flowDocument = value;
                RaisePropertyChanged("FlowDocument");
            }
        }

        public ICommand ReadDocxCommand => new DelegateCommand(() =>
        {
            //var path = @"D:\helloworld.docx";
            var path = @"D:\Template.docx";

           

            var content = docxReader.Read(path);
            DocxContent = content;

        });

        public ICommand RenderDocxCommand => new DelegateCommand(() =>
        {
            //docxReader.ParseStyle();
            docxReader.Render();

            FlowDocument = docxReader.Document;
        });


        public ICommand GenerateDocxCommand => new DelegateCommand(() =>
        {


            var generator = new DocxGenerator();

            byte[] byteArray = Encoding.ASCII.GetBytes(@"<Model>
	<Result>true</Result>
	<Groups>
		<Group>
			<Gro>111</Gro>
			<GroupName>abc</GroupName>
			<Tasks>
				<Task>
					<SourceName>111gg</SourceName>
				</Task>
			</Tasks>
		</Group>
		<Group>
			<Gro>111</Gro>
			<GroupName>abcdd</GroupName>
			<Tasks>
				<Task>
					<SourceName>111gg</SourceName>
				</Task>
			</Tasks>
		</Group>
		<Group>
			<Gro>111</Gro>
			<GroupName>loop</GroupName>
			<Tasks>
				<Task>
					<SourceName>111gg</SourceName>
				</Task>
			</Tasks>
		</Group>
	</Groups>
</Model>");
            MemoryStream stream = new MemoryStream(byteArray);

            var tr = XDocument.Load(stream);
            var mainStream =  generator.Generate(docxReader.MainPartStream, tr);

            var content = docxReader.ReadDocumentStream(mainStream);
            DocxContent = content;

            docxReader.Render(mainStream);

            FlowDocument = docxReader.Document;

        });

        public ICommand SaveDocxCommand => new DelegateCommand(() =>
        {
            string originalFile= @"D:\Template.docx";
            string filePath= @"D:\Template1.docx";

            var generator = new DocxGenerator();

            byte[] byteArray = Encoding.ASCII.GetBytes(@"<Model>
	<Result>true</Result>
	<Groups>
		<Group>
			<Gro>111</Gro>
			<GroupName>abc</GroupName>
			<Tasks>
				<Task>
					<SourceName>111gg</SourceName>
				</Task>
			</Tasks>
		</Group>
		<Group>
			<Gro>111</Gro>
			<GroupName>abcdd</GroupName>
			<Tasks>
				<Task>
					<SourceName>111gg</SourceName>
				</Task>
			</Tasks>
		</Group>
		<Group>
			<Gro>111</Gro>
			<GroupName>loop</GroupName>
			<Tasks>
				<Task>
					<SourceName>111gg</SourceName>
				</Task>
			</Tasks>
		</Group>
	</Groups>
</Model>");
            MemoryStream stream = new MemoryStream(byteArray);

            var tr = XDocument.Load(stream);
            var doc = generator.GenerateXDocument(docxReader.MainPartStream, tr);

            using (var templateStream = new FileStream(originalFile, FileMode.Open))
            {
                using (var destinationStream = new FileStream(filePath, FileMode.Create))
                {
                    templateStream.Seek(0, SeekOrigin.Begin);
                    templateStream.CopyTo(destinationStream);


                    using (var package = Package.Open(templateStream, FileMode.Open, FileAccess.ReadWrite))
                    {
                        var mainpackage = DocxPackageHelper.GetDocumentPart(package);
                        var part = DocxPackageHelper.GetPart(package, new Uri(mainpackage.Uri.OriginalString, UriKind.Relative));
                        var newFilestream = part.GetStream();
                        newFilestream.SetLength(0);
                        using (var writer = new XmlTextWriter(newFilestream, new UTF8Encoding()))
                        {
                            doc.Save(writer);
                        }
                        package.Flush();
                    }

                }
            }


        });




        public DocxWidgetViewModel()
        {
             docxReader = new DocxReader();
        }
    }
}
