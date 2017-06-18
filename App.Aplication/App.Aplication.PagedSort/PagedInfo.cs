using App.Aplication.PagedSort.SortUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace App.Aplication.PagedSort
{
	public class PagedInfo : IPagedInfo
	{
		public const int MaxSortSpecifications = 3;

		public const int DefaultPageSize = 20;

		public readonly static int[] PageSizes;

		private int m_pageNo = 1;

		private int m_TotalItems;

		private int m_PageSize = 20;

		public bool IsFirstPage
		{
			get
			{
				return this.PageNo <= 1;
			}
		}

		public bool IsLastPage
		{
			get
			{
				return this.PageNo >= this.TotalPages;
			}
		}

		public int PageNo
		{
			get
			{
				return JustDecompileGenerated_get_PageNo();
			}
			set
			{
				JustDecompileGenerated_set_PageNo(value);
			}
		}

		public int JustDecompileGenerated_get_PageNo()
		{
			return this.m_pageNo;
		}

		public void JustDecompileGenerated_set_PageNo(int value)
		{
			this.m_pageNo = (value < 1 ? 1 : value);
		}

		public int PageSize
		{
			get
			{
				return this.m_PageSize;
			}
			set
			{
				int num = value;
				if (!PagedInfo.PageSizes.Contains<int>(value))
				{
					num = PagedInfo.PageSizes.FirstOrDefault<int>((int size) => size > value);
					if (num < 1)
					{
						num = PagedInfo.PageSizes.Last<int>();
					}
				}
				this.m_PageSize = num;
			}
		}

		public string SortMetaData { get; set; } = string.Empty;

		public int TotalItems
		{
			get
			{
				return this.m_TotalItems;
			}
			set
			{
				this.m_TotalItems = (value < 0 ? 0 : value);
			}
		}

		public int TotalPages
		{
			get
			{
				return (int)Math.Ceiling(decimal.Parse( TotalItems.ToString()) / this.PageSize);
			}
		}

		static PagedInfo()
		{
			PagedInfo.PageSizes = new int[] { 5, 10, 20, 50, 100 };
		}

		public PagedInfo()
		{
		}

		public PagedInfo(string sortTitle, string sortExpression) : this(sortTitle, sortExpression, SortDirection.Ascending)
		{
		}

		public PagedInfo(string sortTitle, string sortExpression, SortDirection sortDirection)
		{
			this.AddSortExpression(sortTitle, sortExpression, sortDirection);
		}

		public PagedInfo AddSortExpression(string metaData)
		{
			this.AddSortExpression(SortExpression.DeSerialize(metaData));
			return this;
		}

		public PagedInfo AddSortExpression(string title, string sortExpression, SortDirection direction  )
		{
			this.AddSortExpression(new SortExpression(title, sortExpression, direction));
			return this;
		}

		public PagedInfo AddSortExpression(SortExpression sortExpression)
		{
			this.SortMetaData = PagedInfo.AddSortExpression(this.SortMetaData, sortExpression);
			return this;
		}

		public static string AddSortExpression(string sortMetaData, SortExpression sortExpression)
		{
			SortExpressionCollection sortExpressions = PagedInfo.GetSortExpressions(sortMetaData);
			int num = sortExpressions.FindIndex((SortExpression s) => s.Expression == sortExpression.Expression);
			if (num != 0)
			{
				if (num > 0)
				{
					sortExpressions.RemoveAt(num);
				}
				sortExpressions.Insert(0, sortExpression);
				if (sortExpressions.Count > 3)
				{
					sortExpressions.RemoveRange(3, 1);
				}
			}
			else
			{
				sortExpressions[0].ToggleDirection();
			}
			return sortExpressions.Serialize();
		}

		public void ClearSortExpressions()
		{
			this.SortMetaData = string.Empty;
		}

		public static int GetNearestPageSize(int targetSize)
		{
			for (int i = 0; i < (int)PagedInfo.PageSizes.Length; i++)
			{
				int pageSizes = PagedInfo.PageSizes[i];
				if (pageSizes > targetSize)
				{
					if (i == 0)
					{
						return pageSizes;
					}
					int num = PagedInfo.PageSizes[i - 1];
					if (targetSize - num >= pageSizes - targetSize)
					{
						return pageSizes;
					}
					return num;
				}
			}
			return PagedInfo.PageSizes.Last<int>();
		}

		public string GetSortDescription()
		{
			return PagedInfo.GetSortDescription(this.SortMetaData);
		}

		public static string GetSortDescription(string sortMetaData)
		{
			return PagedInfo.GetSortExpressions(sortMetaData).ToString();
		}

		public SortExpressionCollection GetSortExpressions()
		{
			return PagedInfo.GetSortExpressions(this.SortMetaData);
		}

		public static SortExpressionCollection GetSortExpressions(string sortMetaData)
		{
			return SortExpressionCollection.DeSerialize(sortMetaData);
		}
	}
}