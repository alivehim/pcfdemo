using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup.Localizer;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate;

namespace UtilityTools.OpenXml.Extensions
{
    internal static class XElementExtensions
    {

        /***
         * 
         * 
         * 
         * <w:sdt>
			<w:sdtPr>
				<w:rPr>
					<w:lang w:val="en-US"/>
				</w:rPr>
				<w:alias w:val="Repeater"/>
				<w:tag w:val="Repeater"/>
				<w:id w:val="8129370"/>
				<w:placeholder>
					<w:docPart w:val="0E63D105F1664216B36DA9CA21EBF132"/>
				</w:placeholder>
				<w:showingPlcHdr/>
				<w:text/>
			</w:sdtPr>
			<w:sdtEndPr/>
			<w:sdtContent>
				<w:p w:rsidR="00D8307B" w:rsidRPr="008E5001" w:rsidRDefault="001927F4" w:rsidP="00D766A1">
					<w:r>
						<w:rPr>
							<w:rFonts w:ascii="Segoe UI" w:hAnsi="Segoe UI" w:cs="Segoe UI"/>
							<w:color w:val="000000"/>
							<w:sz w:val="18"/>
							<w:szCs w:val="18"/>
						</w:rPr>
						<w:t>//Model</w:t>
					</w:r>
					<w:r w:rsidR="008B12D9">
						<w:rPr>
							<w:rFonts w:ascii="Segoe UI" w:hAnsi="Segoe UI" w:cs="Segoe UI"/>
							<w:color w:val="000000"/>
							<w:sz w:val="18"/>
							<w:szCs w:val="18"/>
						</w:rPr>
						<w:t>/G</w:t>
					</w:r>
					<w:r w:rsidR="003217E0">
						<w:rPr>
							<w:rFonts w:ascii="Segoe UI" w:hAnsi="Segoe UI" w:cs="Segoe UI"/>
							<w:color w:val="000000"/>
							<w:sz w:val="18"/>
							<w:szCs w:val="18"/>
						</w:rPr>
						<w:t>roups/</w:t>
					</w:r>
					<w:r w:rsidR="008B12D9">
						<w:rPr>
							<w:rFonts w:ascii="Segoe UI" w:hAnsi="Segoe UI" w:cs="Segoe UI"/>
							<w:color w:val="000000"/>
							<w:sz w:val="18"/>
							<w:szCs w:val="18"/>
							<w:lang w:val="en-US"/>
						</w:rPr>
						<w:t>G</w:t>
					</w:r>
					<w:r w:rsidR="003217E0">
						<w:rPr>
							<w:rFonts w:ascii="Segoe UI" w:hAnsi="Segoe UI" w:cs="Segoe UI"/>
							<w:color w:val="000000"/>
							<w:sz w:val="18"/>
							<w:szCs w:val="18"/>
						</w:rPr>
						<w:t>roup</w:t>
					</w:r>
				</w:p>
			</w:sdtContent>
		</w:sdt>
         * 
         * 
         */
        public static bool IsSdt(this XElement self)
        {
            return self.Name.Equals(NamespaceConsts.SdtName);
        }

        public static string GetExpression(this XElement element)
        {
            return element.Value.ToString();
        }


        /// <summary>
        /// get the tag name from XElement 
        /// </summary>
        /// <param name="xElement"></param>
        /// <returns></returns>
        public static string GetTagName(this XElement xElement)
        {
            return xElement.Descendants(NamespaceConsts.TagName)
                .First()
                .Attribute(NamespaceConsts.ValAttributeName)?.Value ?? string.Empty;
        }

        public static bool IsTextItem(this XElement xElement)
        {
            return xElement.IsSdt() && SdtTag.ItemText.ToString().ToLower() == xElement.GetTagName().ToLower();
        }

        /// <summary>
        /// get the all desendant elements with name std
        /// </summary>
        /// <param name="startElement"></param>
        /// <returns></returns>
        public static IList<XElement> GetSdtElements(this XElement startElement)
        {
            return startElement.Descendants(NamespaceConsts.SdtName).ToList();
        }

        /// <summary>
        /// get sibling elements with tag 
        /// </summary>
        /// <param name="xElement"></param>
        /// <returns></returns>
        public static IList<XElement> GetNextTagElements(this XElement xElement)
        {
            var sdtElements = xElement.ElementsAfterSelf(NamespaceConsts.SdtName);

            var res = from m in sdtElements
                      where m.Descendants(NamespaceConsts.TagName)
                      .First().Attribute(NamespaceConsts.ValAttributeName)?.Value != string.Empty

                      select m;

            return res.ToList();
        }

        public static XElement? GetNextTagElement(this XElement xElement)
        {
            var sdtElements = xElement.ElementsAfterSelf(NamespaceConsts.SdtName);

            var res = from m in sdtElements
                      where m.Descendants(NamespaceConsts.TagName)
                      .First().Attribute(NamespaceConsts.ValAttributeName)?.Value != string.Empty

                      select m;

            return res.FirstOrDefault();
        }

        public static IList<XElement> GetNextTagElements(this XElement xElement, SdtTag sdtTag)
        {
            var sdtElements = xElement.ElementsAfterSelf(NamespaceConsts.SdtName);

            var res = from m in sdtElements
                      where m.Descendants(NamespaceConsts.TagName)
                      .First().Attribute(NamespaceConsts.ValAttributeName)?.Value == sdtTag.ToString().ToLower()

                      select m;

            return res.ToList();
        }


        public static IList<XElement> GetInnerTagElements(this XElement xElement, XElement endElement, IList<string>? tagList = null)
        {
            var elementswithTag = xElement.GetNextTagElements();
            //var tagList = new List<string> { "if", "repeater" };
            IList<XElement> result = new List<XElement>();

            foreach (var element in elementswithTag)
            {
                if (element == endElement)
                    break;


                if (tagList != null && !tagList.Any())
                {
                    var tagName = element.GetTagName();
                    if (tagList.Any(p => p == tagName))
                    {
                        result.Add(element);
                    }
                }
                else
                {
                    result.Add(element);
                }

            }

            return result;
        }

        public static IList<XElement> GetInnerElements(this XElement startElement, XElement endElement)
        {
            var elements = startElement.ElementsAfterSelf();

            IList<XElement> result = new List<XElement>();

            foreach (var element in elements)
            {
                if (element == endElement)
                    break;

                result.Add(element);


            }

            return result;

        }

  
    }
}
