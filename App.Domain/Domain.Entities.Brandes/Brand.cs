using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Brandes
{
	public class Brand : AuditableEntity<int>
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

		public int Status
		{
			get;
			set;
		}

        public virtual ICollection<Order> Orders
        {
            get;
            set;
        }

        public Brand()
		{
		}
	}
}