using App.Core.Common;
using App.Domain.Entities.Ads;
using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Menu
{
	public class MenuLink : AuditableEntity<int>
	{
		public virtual ICollection<Banner> Banners
		{
			get;
			set;
		}

		[StringLength(250)]
		public string CurrentVirtualId
		{
			get;
			set;
		}

		public bool DisplayOnHomePage
		{
			get;
			set;
		}

		public bool DisplayOnMenu
		{
			get;
			set;
		}

		public bool DisplayOnSearch
		{
			get;
			set;
		}

		public string Icon1
		{
			get;
			set;
		}

		public string Icon2
		{
			get;
			set;
		}

		[StringLength(250)]
		public string ImageUrl
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

		[StringLength(250)]
		public string MenuName
		{
			get;
			set;
		}

		[StringLength(550)]
		public string MetaDescription
		{
			get;
			set;
		}

		[StringLength(550)]
		public string MetaKeywords
		{
			get;
			set;
		}

		[StringLength(550)]
		public string MetaTitle
		{
			get;
			set;
		}

		public virtual ICollection<App.Domain.Entities.Data.News> News
		{
			get;
			set;
		}

		public int OrderDisplay
		{
			get;
			set;
		}

		public int? ParentId
		{
			get;
			set;
		}

		public virtual MenuLink ParentMenu
		{
			get;
			set;
		}

		public int Position
		{
			get;
			set;
		}

		public virtual ICollection<Post> Posts
		{
			get;
			set;
		}

		[StringLength(250)]
		public string SeoUrl
		{
			get;
			set;
		}

		[StringLength(250)]
		public string SourceLink
		{
			get;
			set;
		}

		public virtual ICollection<StaticContent> StaticContents
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public int TemplateType
		{
			get;
			set;
		}

		public int TypeMenu
		{
			get;
			set;
		}

		[StringLength(250)]
		public string VirtualId
		{
			get;
			set;
		}

		[StringLength(250)]
		public string VirtualSeoUrl
		{
			get;
			set;
		}
        
        public MenuLink()
		{
		}
	}
}