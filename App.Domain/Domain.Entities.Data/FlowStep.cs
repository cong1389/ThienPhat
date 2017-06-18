using App.Core.Common;
using System;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Data
{
	public class FlowStep : AuditableEntity<int>
	{
		public string Description
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public int OrderDisplay
		{
			get;
			set;
		}

		public string OtherLink
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

		public FlowStep()
		{
		}
	}
}