using App.Core.Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Account
{
	public class User : Entity<Guid>
	{
		private ICollection<Claim> _claims;

		private ICollection<ExternalLogin> _externalLogins;

		private ICollection<Role> _roles;

		public string Address
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public virtual ICollection<Claim> Claims
		{
			get
			{
				ICollection<Claim> claims = this._claims;
				if (claims == null)
				{
					List<Claim> claims1 = new List<Claim>();
					ICollection<Claim> claims2 = claims1;
					this._claims = claims1;
					claims = claims2;
				}
				return claims;
			}
			set
			{
				this._claims = value;
			}
		}

		public DateTime Created
		{
			get;
			set;
		}

		public string DisplayAddress
		{
			get
			{
				string str = (string.IsNullOrEmpty(this.Address) ? string.Empty : this.Address);
				string str1 = (string.IsNullOrEmpty(this.City) ? string.Empty : this.City);
				return string.Format("{0} {1} {2}", str, str1, (string.IsNullOrEmpty(this.State) ? string.Empty : this.State));
			}
		}

		public string DisplayName
		{
			get
			{
				string str = (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName);
				string str1 = (string.IsNullOrEmpty(this.MiddleName) ? string.Empty : this.MiddleName);
				return string.Format("{0} {1} {2}", (string.IsNullOrEmpty(this.LastName) ? string.Empty : this.LastName), str1, str);
			}
		}

		public string Email
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public bool IsLockedOut
		{
			get;
			set;
		}

		public bool IsSuperAdmin
		{
			get;
			set;
		}

		public DateTime? LastLogin
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public virtual ICollection<ExternalLogin> Logins
		{
			get
			{
				ICollection<ExternalLogin> externalLogins = this._externalLogins;
				if (externalLogins == null)
				{
					List<ExternalLogin> externalLogins1 = new List<ExternalLogin>();
					ICollection<ExternalLogin> externalLogins2 = externalLogins1;
					this._externalLogins = externalLogins1;
					externalLogins = externalLogins2;
				}
				return externalLogins;
			}
			set
			{
				this._externalLogins = value;
			}
		}

		public string MiddleName
		{
			get;
			set;
		}

		public virtual string PasswordHash
		{
			get;
			set;
		}

		public string Phone
		{
			get;
			set;
		}

		public virtual ICollection<Role> Roles
		{
			get
			{
				ICollection<Role> roles = this._roles;
				if (roles == null)
				{
					List<Role> roles1 = new List<Role>();
					ICollection<Role> roles2 = roles1;
					this._roles = roles1;
					roles = roles2;
				}
				return roles;
			}
			set
			{
				this._roles = value;
			}
		}

		public virtual string SecurityStamp
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public User()
		{
		}
	}
}