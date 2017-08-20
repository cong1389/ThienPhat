using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Extensions
{
    public static class StringExtensions
    {
        public const string CarriageReturnLineFeed = "\r\n";
        public const string Empty = "";
        public const char CarriageReturn = '\r';
        public const char LineFeed = '\n';
        public const char Tab = '\t';
        

        #region String extensions
        

        [Obsolete("The 'removeTags' parameter is not supported anymore. Use the parameterless method instead.")]
        public static string RemoveHtml(this string source, ICollection<string> removeTags)
        {
            return RemoveHtml(source);
        }

        public static string RemoveHtml(this string source)
        {
            if (source.IsEmpty())
                return string.Empty;

            var doc = new HtmlDocument()
            {
                OptionOutputOriginalCase = true,
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };

            doc.LoadHtml(source);
            var nodes = doc.DocumentNode.Descendants().Where(n =>
               n.NodeType == HtmlNodeType.Text &&
               n.ParentNode.Name != "script" &&
               n.ParentNode.Name != "style" &&
               n.ParentNode.Name != "svg");

            var sb = new StringBuilder();
            foreach (var node in nodes)
            {
                var text = node.InnerText;
                if (text.HasValue())
                {
                    sb.AppendLine(node.InnerText);
                }
            }

            return sb.ToString().HtmlDecode();
        }

        #endregion

        [DebuggerStepThrough]
        public static string HtmlDecode(this string value)
        {
            return HttpUtility.HtmlDecode(value);
        }

        [DebuggerStepThrough]
        public static string NullEmpty(this string value)
        {
            return (string.IsNullOrEmpty(value)) ? null : value;
        }

        [DebuggerStepThrough]
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        public static bool IsCaseInsensitiveEqual(this string value, string comparing)
        {
            return string.Compare(value, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }

        [DebuggerStepThrough]
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
