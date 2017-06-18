using App.Core.Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Attribute
{
	public class Attribute : AuditableEntity<int>
	{
		public string AttributeName
		{
			get;
			set;
		}

		public virtual ICollection<AttributeValue> AttributeValues
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int? OrderDisplay
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public Attribute()
		{
		}
	}
}