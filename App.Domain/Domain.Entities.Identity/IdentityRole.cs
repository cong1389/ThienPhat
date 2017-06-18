using Microsoft.AspNet.Identity;
using System;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Identity
{
	public class IdentityRole : IRole<Guid>
	{
		public string Description
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

		public string Name
		{
			get;
			set;
		}

		public IdentityRole()
		{
			this.Id = Guid.NewGuid();
		}

		public IdentityRole(string name) : this()
		{
			this.Name = name;
		}

		public IdentityRole(string name, string description, Guid id)
		{
			this.Name = name;
			this.Description = description;
			this.Id = id;
		}
	}
}