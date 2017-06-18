using App.SeoSitemap.StyleSheets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace App.SeoSitemap.Serialization
{
	internal class XmlSerializer : IXmlSerializer
	{
		private readonly IXmlNamespaceBuilder _xmlNamespaceBuilder;

		private readonly XmlProcessingInstructionHandler _xmlProcessingInstructionHandler;

		public XmlSerializer()
		{
			this._xmlNamespaceBuilder = new XmlNamespaceBuilder();
			this._xmlProcessingInstructionHandler = new XmlProcessingInstructionHandler();
		}

		public string Serialize<T>(T data)
		{
			StringWriter stringWriterWithEncoding = new StringWriterWithEncoding(Encoding.UTF8);
			this.SerializeToStream<T>(data, (XmlWriterSettings settings) => XmlWriter.Create(stringWriterWithEncoding, settings));
			return stringWriterWithEncoding.ToString();
		}

		public void SerializeToStream<T>(T data, Stream stream)
		{
			this.SerializeToStream<T>(data, (XmlWriterSettings settings) => XmlWriter.Create(stream, settings));
		}

		private void SerializeToStream<T>(T data, Func<XmlWriterSettings, XmlWriter> createXmlWriter)
		{
			IEnumerable<string> strs;
			IXmlNamespaceProvider xmlNamespaceProvider = (object)data as IXmlNamespaceProvider;
			strs = (xmlNamespaceProvider != null ? xmlNamespaceProvider.GetNamespaces() : Enumerable.Empty<string>());
			XmlSerializerNamespaces xmlSerializerNamespace = this._xmlNamespaceBuilder.Create(strs);
			System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
			using (XmlWriter xmlWriter = createXmlWriter(new XmlWriterSettings()
			{
				Encoding = Encoding.UTF8,
				NamespaceHandling = NamespaceHandling.OmitDuplicates
			}))
			{
				if ((object)data is IHasStyleSheets)
				{
					this._xmlProcessingInstructionHandler.AddStyleSheets(xmlWriter, (object)data as IHasStyleSheets);
				}
				xmlSerializer.Serialize(xmlWriter, data, xmlSerializerNamespace);
			}
		}
	}
}