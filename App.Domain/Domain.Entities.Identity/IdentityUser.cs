using Microsoft.AspNet.Identity;
using System;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Identity
{
	public class IdentityUser : IUser<Guid>
	{
		public virtual string Address
		{
			get;
			set;
		}

		public virtual string City
		{
			get;
			set;
		}

		public DateTime Created
		{
			get;
			set;
		}

		public virtual string Email
		{
			get;
			set;
		}

		public virtual string FirstName
		{
			get;
			set;
		}

		public Guid Id
		{
			get
			{
				return JustDecompileGenerated_get_Id();
			}
			set
			{
				JustDecompileGenerated_set_Id(value);
			}
		}

		private Guid JustDecompileGenerated_Id_k__BackingField;

		public Guid JustDecompileGenerated_get_Id()
		{
			return this.JustDecompileGenerated_Id_k__BackingField;
		}

		public void JustDecompileGenerated_set_Id(Guid value)
		{
			this.JustDecompileGenerated_Id_k__BackingField = value;
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

		public virtual string LastName
		{
			get;
			set;
		}

		public virtual string MiddleName
		{
			get;
			set;
		}

		public virtual string PasswordHash
		{
			get;
			set;
		}

		public virtual string Phone
		{
			get;
			set;
		}

		public virtual string SecurityStamp
		{
			get;
			set;
		}

		public virtual string State
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public IdentityUser()
		{
			this.Id = Guid.NewGuid();
		}

		public IdentityUser(string userName, string email, string firstName, string lastName, string middleName, string phone, string addess, string city, string state, bool superAdmin, bool isLockedOut, DateTime createdDate) : this()
		{
			this.UserName = userName;
			this.Email = email;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.MiddleName = middleName;
			this.Phone = phone;
			this.Address = addess;
			this.City = city;
			this.State = state;
			this.IsSuperAdmin = superAdmin;
			this.Created = createdDate;
			this.IsLockedOut = isLockedOut;
		}
	}
}