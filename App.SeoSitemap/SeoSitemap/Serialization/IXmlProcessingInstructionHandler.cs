using App.SeoSitemap.StyleSheets;
using System;
using System.Xml;

namespace App.SeoSitemap.Serialization
{
	internal interface IXmlProcessingInstructionHandler
	{
		void AddStyleSheets(XmlWriter xmlWriter, IHasStyleSheets model);
	}
}