using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace App.SeoSitemap.Serialization
{
	internal sealed class StringWriterWithEncoding : StringWriter
	{
		public override System.Text.Encoding Encoding
		{
			get;
		}

		public StringWriterWithEncoding(System.Text.Encoding encoding)
		{
			this.Encoding = encoding;
		}
	}
}