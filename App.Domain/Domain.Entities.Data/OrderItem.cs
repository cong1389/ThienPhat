using App.Core.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Data
{
	public class OrderItem : AuditableEntity<int>
	{
		public string Category
		{
			get;
			set;
		}

		public decimal? FixedFee
		{
			get;
			set;
		}

		[ForeignKey("OrderId")]
		public virtual App.Domain.Entities.Data.Order Order
		{
			get;
			set;
		}

		public int OrderId
		{
			get;
			set;
		}

		public DateTime? WarrantyFrom
		{
			get;
			set;
		}

		public DateTime? WarrantyTo
		{
			get;
			set;
		}

		public OrderItem()
		{
		}
	}
}