using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace WebCache.Static
{
	public class StaticContentFilter : Stream
	{
		private readonly static char[] HREF_ATTRIBUTE;

		private readonly static char[] REL_ATTRIBUTE;

		private readonly static char[] HTTP_PREFIX;

		private readonly static char[] IMG_TAG;

		private readonly static char[] LINK_TAG;

		private readonly static char[] SCRIPT_TAG;

		private readonly static char[] SRC_ATTRIBUTE;

		private byte[] _CssPrefix;

		private Encoding _Encoding;

		private byte[] _ImagePrefix;

		private byte[] _JavascriptPrefix;

		private char[] _ApplicationPath;

		private byte[] _BaseUrl;

		private byte[] _CurrentFolder;

		private char[] _PendingBuffer;

		private Stream _ResponseStream;

		private Func<string, string> _getVersionOfFile;

		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		public override long Length
		{
			get
			{
				return (long)0;
			}
		}

		public override long Position
		{
			get;
			set;
		}

		static StaticContentFilter()
		{
			StaticContentFilter.HREF_ATTRIBUTE = "href".ToCharArray();
			StaticContentFilter.REL_ATTRIBUTE = "rel".ToCharArray();
			StaticContentFilter.HTTP_PREFIX = "http://".ToCharArray();
			StaticContentFilter.IMG_TAG = "img".ToCharArray();
			StaticContentFilter.LINK_TAG = "link".ToCharArray();
			StaticContentFilter.SCRIPT_TAG = "script".ToCharArray();
			StaticContentFilter.SRC_ATTRIBUTE = "src".ToCharArray();
		}

		public StaticContentFilter(HttpResponse response, Func<string, string> getVersionOfFile, string imagePrefix, string javascriptPrefix, string cssPrefix, string baseUrl, string applicationPath, string currentRelativePath)
		{
			this._Encoding = response.Output.Encoding;
			this._ResponseStream = response.Filter;
			this._ImagePrefix = this._Encoding.GetBytes(imagePrefix);
			this._JavascriptPrefix = this._Encoding.GetBytes(javascriptPrefix);
			this._CssPrefix = this._Encoding.GetBytes(cssPrefix);
			this._ApplicationPath = applicationPath.ToCharArray();
			this._BaseUrl = this._Encoding.GetBytes(baseUrl);
			this._CurrentFolder = this._Encoding.GetBytes(currentRelativePath);
			this._getVersionOfFile = getVersionOfFile;
		}

		public override void Close()
		{
			this.FlushPendingBuffer();
			this._ResponseStream.Close();
			this._ResponseStream = null;
			this._getVersionOfFile = null;
			this._PendingBuffer = null;
		}

		private int FindAttributeValuePos(char[] attributeName, char[] content, int pos)
		{
			for (int i = pos; i < (int)content.Length - (int)attributeName.Length; i++)
			{
				if (62 == content[i])
				{
					return -1;
				}
				if (this.HasMatch(content, i, attributeName))
				{
					pos = i + (int)attributeName.Length;
					int num = pos;
					pos = num + 1;
					if (34 != content[num])
					{
						return pos;
					}
				}
			}
			return -1;
		}

		public override void Flush()
		{
			this.FlushPendingBuffer();
			this._ResponseStream.Flush();
		}

		private void FlushPendingBuffer()
		{
			if (this._PendingBuffer != null)
			{
				this.WriteOutput(this._PendingBuffer, 0, (int)this._PendingBuffer.Length);
				this._PendingBuffer = null;
			}
		}

		private bool HasMatch(char[] content, int pos, char[] match)
		{
			for (int i = 0; i < (int)match.Length; i++)
			{
				if (content[pos + i] != match[i] && content[pos + i] != match[i])
				{
					return false;
				}
			}
			return true;
		}

		private bool HasTagEnd(char[] content, int pos)
		{
			while (pos < (int)content.Length)
			{
				if (62 == content[pos])
				{
					return true;
				}
				pos++;
			}
			return false;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._ResponseStream.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._ResponseStream.Seek(offset, origin);
		}

		public override void SetLength(long length)
		{
			this._ResponseStream.SetLength(length);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			char[] chrArray;
			char[] chars = this._Encoding.GetChars(buffer, offset, count);
			if (this._PendingBuffer == null)
			{
				chrArray = chars;
			}
			else
			{
				chrArray = new char[(int)chars.Length + (int)this._PendingBuffer.Length];
				Array.Copy(this._PendingBuffer, 0, chrArray, 0, (int)this._PendingBuffer.Length);
				Array.Copy(chars, 0, chrArray, (int)this._PendingBuffer.Length, (int)chars.Length);
				this._PendingBuffer = null;
			}
			int num = 0;
			for (int i = 0; i < (int)chrArray.Length; i++)
			{
				if (60 == chrArray[i])
				{
					i++;
					if (!this.HasTagEnd(chrArray, i))
					{
						this._PendingBuffer = new char[(int)chrArray.Length - i];
						Array.Copy(chrArray, i, this._PendingBuffer, 0, (int)chrArray.Length - i);
						this.WriteOutput(chrArray, num, i - num);
						return;
					}
					if (47 != chrArray[i])
					{
						if (this.HasMatch(chrArray, i, StaticContentFilter.IMG_TAG))
						{
							num = this.WritePrefixIf(StaticContentFilter.SRC_ATTRIBUTE, chrArray, i, num, this._ImagePrefix);
						}
						else if (this.HasMatch(chrArray, i, StaticContentFilter.SCRIPT_TAG))
						{
							num = this.WritePrefixIf(StaticContentFilter.SRC_ATTRIBUTE, chrArray, i, num, this._JavascriptPrefix);
							num = this.WritePathWithVersion(chrArray, num);
						}
						else if (this.HasMatch(chrArray, i, StaticContentFilter.LINK_TAG))
						{
							num = this.WritePrefixIf(StaticContentFilter.HREF_ATTRIBUTE, chrArray, i, num, this._CssPrefix);
							num = this.WritePathWithVersion(chrArray, num);
						}
						if (num > i)
						{
							i = num;
						}
					}
				}
			}
			this.WriteOutput(chrArray, num, (int)chrArray.Length - num);
		}

		private void WriteBytes(byte[] bytes, int pos, int length)
		{
			this._ResponseStream.Write(bytes, 0, (int)bytes.Length);
		}

		private void WriteOutput(char[] content, int pos, int length)
		{
			if (length == 0)
			{
				return;
			}
			byte[] bytes = this._Encoding.GetBytes(content, pos, length);
			this.WriteBytes(bytes, 0, (int)bytes.Length);
		}

		private int WritePathWithVersion(char[] content, int lastPosWritten)
		{
			if (!this.HasMatch(content, lastPosWritten, StaticContentFilter.HTTP_PREFIX))
			{
				int num = lastPosWritten + 1;
				while (34 != content[num])
				{
					num++;
				}
				string str = new string(content, lastPosWritten, num - lastPosWritten);
				this.WriteOutput(content, lastPosWritten, num - lastPosWritten);
				lastPosWritten = num;
				char[] charArray = this._getVersionOfFile(str).ToCharArray();
				this.WriteOutput(charArray, 0, (int)charArray.Length);
			}
			return lastPosWritten;
		}

		private int WritePrefixIf(char[] attributeName, char[] content, int pos, int lastWritePos, byte[] prefix)
		{
			int length = this.FindAttributeValuePos(attributeName, content, pos);
			if (length <= 0)
			{
				return lastWritePos;
			}
			if (this.HasMatch(content, length, StaticContentFilter.HTTP_PREFIX))
			{
				return lastWritePos;
			}
			this.WriteOutput(content, lastWritePos, length - lastWritePos);
			if (prefix.Length != 0)
			{
				this.WriteBytes(prefix, 0, (int)prefix.Length);
			}
			if (this.HasMatch(content, length, this._ApplicationPath))
			{
				length = length + (int)this._ApplicationPath.Length;
			}
			else if (this._CurrentFolder.Length != 0)
			{
				this.WriteBytes(this._CurrentFolder, 0, (int)this._CurrentFolder.Length);
			}
			if (47 == content[length])
			{
				length++;
			}
			return length;
		}
	}
}