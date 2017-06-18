using App.Core.Common;
using App.Domain.Entities.Menu;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Ads
{
	public class Banner : AuditableEntity<int>
	{
		public TimeSpan? FromDate
		{
			get;
			set;
		}

		[StringLength(50)]
		public string Height
		{
			get;
			set;
		}

		public string ImgPath
		{
			get;
			set;
		}

		[StringLength(5)]
		public string Language
		{
			get;
			set;
		}

		public int? MenuId
		{
			get;
			set;
		}

		[ForeignKey("MenuId")]
		public virtual App.Domain.Entities.Menu.MenuLink MenuLink
		{
			get;
			set;
		}

		public int OrderDisplay
		{
			get;
			set;
		}

		[ForeignKey("PageId")]
		public virtual App.Domain.Entities.Ads.PageBanner PageBanner
		{
			get;
			set;
		}

		public int PageId
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		[StringLength(50)]
		public string Target
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public TimeSpan? ToDate
		{
			get;
			set;
		}

		[StringLength(250)]
		public string WebsiteLink
		{
			get;
			set;
		}

		[StringLength(50)]
		public string Width
		{
			get;
			set;
		}

		public Banner()
		{
		}
	}
}