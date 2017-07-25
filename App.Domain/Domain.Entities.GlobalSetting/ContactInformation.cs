using App.Core.Common;
using App.Domain.Entities.Location;
using App.Domain.Entities.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.GlobalSetting
{
	public class ContactInformation : AuditableEntity<int>
	{
		[StringLength(250)]
		public string Address
		{
			get;
			set;
		}

		[StringLength(50)]
		public string Email
		{
			get;
			set;
		}

		[StringLength(50)]
		public string Fax
		{
			get;
			set;
		}

		[StringLength(50)]
		public string Hotline
		{
			get;
			set;
		}

		[StringLength(50)]
		public string Lag
		{
			get;
			set;
		}

		public virtual ICollection<LandingPage> LandingPages
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
		public string Lat
		{
			get;
			set;
		}

		[StringLength(50)]
		public string MobilePhone
		{
			get;
			set;
		}

		public string NumberOfStore
		{
			get;
			set;
		}

		public int OrderDisplay
		{
			get;
			set;
		}

		[ForeignKey("ProvinceId")]
		public virtual App.Domain.Entities.Location.Province Province
		{
			get;
			set;
		}

		public int? ProvinceId
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public ContactInformation()
		{
		}
	}
}