using App.Core.Common;
using System;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Location
{
	public class District : AuditableEntity<int>
	{
		public string Description
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int OrderDisplay
		{
			get;
			set;
		}

		public int ProvinceId
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public District()
		{
		}
	}
}