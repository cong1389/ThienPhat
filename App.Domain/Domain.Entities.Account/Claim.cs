using App.Core.Common;
using System;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Account
{
	public class Claim : Entity<int>
	{
		private App.Domain.Entities.Account.User _user;

		public virtual string ClaimType
		{
			get;
			set;
		}

		public virtual string ClaimValue
		{
			get;
			set;
		}

		public virtual App.Domain.Entities.Account.User User
		{
			get
			{
				return this._user;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._user = value;
				this.UserId = value.Id;
			}
		}

		public virtual Guid UserId
		{
			get;
			set;
		}

		public Claim()
		{
		}
	}
}