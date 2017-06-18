using App.SeoSitemap.Enum;
using App.SeoSitemap.StyleSheets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace App.SeoSitemap.Serialization
{
	internal class XmlProcessingInstructionHandler : IXmlProcessingInstructionHandler
	{
		public XmlProcessingInstructionHandler()
		{
		}

		public void AddStyleSheets(XmlWriter xmlWriter, IHasStyleSheets model)
		{
			if (model.StyleSheets == null)
			{
				return;
			}
			foreach (XmlStyleSheet styleSheet in model.StyleSheets)
			{
				StringBuilder stringBuilder = new StringBuilder(string.Format("type=\"{0}\" href=\"{1}\"", styleSheet.Type, styleSheet.Url));
				this.WriteAttribute(stringBuilder, "title", styleSheet.Title);
				this.WriteAttribute(stringBuilder, "media", styleSheet.Media);
				this.WriteAttribute(stringBuilder, "charset", styleSheet.Charset);
				if (styleSheet.Alternate.HasValue && styleSheet.Alternate.Value != YesNo.None)
				{
					YesNo value = styleSheet.Alternate.Value;
					this.WriteAttribute(stringBuilder, "alternate", value.ToString().ToLowerInvariant());
				}
				xmlWriter.WriteProcessingInstruction("xml-stylesheet", stringBuilder.ToString());
			}
		}

		private void WriteAttribute(StringBuilder stringBuilder, string attributeName, string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				stringBuilder.Append(string.Format(" {0}=\"{1}\"", attributeName, value));
			}
		}
	}
}