using System;
using System.Runtime.CompilerServices;

namespace App.Framework.Ultis
{
	public class Helper
	{
		public Helper()
		{
		}

		public class PageInfo
		{
			public readonly int Leave;

			private int _limit;

			private int _page;

			public int Begin
			{
				get
				{
					int num = (this._page - 1) * this._limit + 1;
					return num;
				}
			}

			public int CurrentPage
			{
				get
				{
					return this._page;
				}
				set
				{
					this._page = (value < 1 ? 1 : value);
				}
			}

			public int End
			{
				get
				{
					return this._page * this._limit;
				}
			}

			public bool HideJumpBox
			{
				get;
				set;
			}

			public bool HideLimitBox
			{
				get;
				set;
			}

			public int ItemsPerPage
			{
				get
				{
					return this._limit;
				}
				set
				{
					int num;
					if (value < 4)
					{
						num = 4;
					}
					else
					{
						num = (value > 200 ? 200 : value);
					}
					this._limit = num;
				}
			}

			public double TotalItems
			{
				get;
				set;
			}

			public int TotalPage
			{
				get
				{
					return (this.ItemsPerPage > 0 ? (int)Math.Ceiling(this.TotalItems / this.ItemsPerPage) : 0);
				}
			}

			public Func<int, string> Url
			{
				get;
				set;
			}

			public PageInfo()
			{
				this.Url = (int i) => "";
			}

			public PageInfo(int limit, int page) : this(limit, page, 0, false, false, (int u) => string.Empty)
			{
			}

			public PageInfo(int limit, int page, int count, Func<int, string> url) : this(limit, page, count, false, false, url)
			{
			}

			public PageInfo(int limit, int page, int count, bool hlb, bool hjb, Func<int, string> url)
			{
				this.ItemsPerPage = limit;
				this.CurrentPage = page;
				this.TotalItems = count;
				this.HideLimitBox = hlb;
				this.HideJumpBox = hjb;
				this.Url = url;
			}
		}
	}
}