using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.Core.Common
{
	public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
	{
		[MaxLength(256)]
		[ScaffoldColumn(false)]
		public string CreatedBy
		{
			get;
			set;
		}

		[ScaffoldColumn(false)]
		public DateTime CreatedDate
		{
			get;
			set;
		}

		[MaxLength(256)]
		[ScaffoldColumn(false)]
		public string UpdatedBy
		{
			get;
			set;
		}

		[ScaffoldColumn(false)]
		public DateTime? UpdatedDate
		{
			get;
			set;
		}

		protected AuditableEntity()
		{
		}
	}
}