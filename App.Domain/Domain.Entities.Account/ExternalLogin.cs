using App.Core.Common;
using System;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Account
{
	public class ExternalLogin : BaseEntity
	{
		private App.Domain.Entities.Account.User _user;

		public virtual string LoginProvider
		{
			get;
			set;
		}

		public virtual string ProviderKey
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
				this._user = value;
				this.UserId = value.Id;
			}
		}

		public virtual Guid UserId
		{
			get;
			set;
		}

		public ExternalLogin()
		{
		}
	}
}