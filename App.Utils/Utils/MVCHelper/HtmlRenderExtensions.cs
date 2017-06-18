using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace App.Utils.MVCHelper
{
	public static class HtmlRenderExtensions
	{
		public static IDisposable Delayed(this HtmlHelper helper, string injectionBlockId = null, string isOnlyOne = null)
		{
			return new HtmlRenderExtensions.DelayedInjectionBlock(helper, injectionBlockId, isOnlyOne);
		}

		public static MvcHtmlString RenderDelayed(this HtmlHelper helper, string injectionBlockId = null, bool removeAfterRendering = true)
		{
			Queue<string> queue = HtmlRenderExtensions.DelayedInjectionBlock.GetQueue(helper, injectionBlockId);
			if (!removeAfterRendering)
			{
				return MvcHtmlString.Create(string.Join(Environment.NewLine, queue));
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (queue.Count > 0)
			{
				stringBuilder.AppendLine(queue.Dequeue());
			}
			return MvcHtmlString.Create(stringBuilder.ToString());
		}

		private class DelayedInjectionBlock : IDisposable
		{
			private const string CACHE_KEY = "DCCF8C78-2E36-4567-B0CF-FE052ACCE309";

			private const string UNIQUE_IDENTIFIER_KEY = "DCCF8C78-2E36-4567-B0CF-FE052ACCE309";

			private const string EMPTY_IDENTIFIER = "";

			private readonly HtmlHelper helper;

			private readonly string identifier;

			private readonly string isOnlyOne;

			public DelayedInjectionBlock(HtmlHelper helper, string identifier = null, string isOnlyOne = null)
			{
				this.helper = helper;
				((WebViewPage)this.helper.ViewDataContainer).OutputStack.Push(new StringWriter());
				this.identifier = identifier ?? "";
				this.isOnlyOne = isOnlyOne;
			}

			private static T _GetOrSet<T>(HtmlHelper helper, T defaultValue, string identifier = "")
			where T : class
			{
				object item;
				Dictionary<string, object> storage = HtmlRenderExtensions.DelayedInjectionBlock.GetStorage(helper);
				if (storage.ContainsKey(identifier))
				{
					item = storage[identifier];
				}
				else
				{
					object obj = defaultValue;
					object obj1 = obj;
					storage[identifier] = obj;
					item = obj1;
				}
				return (T)item;
			}

			public void Dispose()
			{
				Stack<TextWriter> outputStack = ((WebViewPage)this.helper.ViewDataContainer).OutputStack;
				string str = (outputStack.Count == 0 ? string.Empty : outputStack.Pop().ToString());
				Queue<string> queue = HtmlRenderExtensions.DelayedInjectionBlock.GetQueue(this.helper, this.identifier);
				Dictionary<string, int> count = HtmlRenderExtensions.DelayedInjectionBlock._GetOrSet<Dictionary<string, int>>(this.helper, new Dictionary<string, int>(), "DCCF8C78-2E36-4567-B0CF-FE052ACCE309");
				if (this.isOnlyOne == null || !count.ContainsKey(this.isOnlyOne))
				{
					queue.Enqueue(str);
					if (this.isOnlyOne != null)
					{
						count[this.isOnlyOne] = queue.Count;
					}
				}
			}

			public static Queue<string> GetQueue(HtmlHelper helper, string identifier = null)
			{
				return HtmlRenderExtensions.DelayedInjectionBlock._GetOrSet<Queue<string>>(helper, new Queue<string>(), identifier ?? "");
			}

			public static Dictionary<string, object> GetStorage(HtmlHelper helper)
			{
				Dictionary<string, object> item = helper.ViewContext.HttpContext.Items["DCCF8C78-2E36-4567-B0CF-FE052ACCE309"] as Dictionary<string, object>;
				if (item == null)
				{
					IDictionary items = helper.ViewContext.HttpContext.Items;
					Dictionary<string, object> strs = new Dictionary<string, object>();
					item = strs;
					items["DCCF8C78-2E36-4567-B0CF-FE052ACCE309"] = strs;
				}
				return item;
			}
		}
	}
}