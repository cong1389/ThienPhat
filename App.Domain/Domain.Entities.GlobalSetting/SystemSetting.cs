using App.Core.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.GlobalSetting
{
	public class SystemSetting : AuditableEntity<int>
	{
		public string Description
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		[StringLength(50)]
		public string Favicon
		{
			get;
			set;
		}

		[StringLength(450)]
		public string FooterContent
		{
			get;
			set;
		}

		public string Hotline
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

		[StringLength(50)]
		public string LogoImage
		{
			get;
			set;
		}

		public bool MaintanceSite
		{
			get;
			set;
		}

		[StringLength(450)]
		public string MetaDescription
		{
			get;
			set;
		}

		[StringLength(250)]
		public string MetaKeywords
		{
			get;
			set;
		}

		[StringLength(250)]
		public string MetaTitle
		{
			get;
			set;
		}

		public string Slogan
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		[StringLength(250)]
		public string TimeWork
		{
			get;
			set;
		}

		[StringLength(450)]
		public string Title
		{
			get;
			set;
		}

		public SystemSetting()
		{
		}
	}
}