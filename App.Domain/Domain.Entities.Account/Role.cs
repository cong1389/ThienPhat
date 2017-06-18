using App.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Account
{
	public class Role : Entity<Guid>
	{
		private ICollection<User> _users;

		[StringLength(450)]
		public string Description
		{
			get;
			set;
		}

		[StringLength(250)]
		public string Name
		{
			get;
			set;
		}

		public ICollection<User> Users
		{
			get
			{
				ICollection<User> users = this._users;
				if (users == null)
				{
					List<User> users1 = new List<User>();
					ICollection<User> users2 = users1;
					this._users = users1;
					users = users2;
				}
				return users;
			}
			set
			{
				this._users = value;
			}
		}

		public Role()
		{
		}
	}
}