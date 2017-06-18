using App.Core.Common;
using App.Domain.Entities.Account;
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Mapping
{
	public class RoleConfiguration : EntityTypeConfiguration<Role>
	{
		public RoleConfiguration()
		{
			base.ToTable("Role");
			base.HasKey<Guid>((Role x) => x.Id).Property<Guid>((Role x) => x.Id).HasColumnName("RoleId").HasColumnType("uniqueidentifier").IsRequired();
			base.HasMany<User>((Role x) => x.Users).WithMany((User x) => x.Roles).Map((ManyToManyAssociationMappingConfiguration x) => {
				x.ToTable("UserRole");
				x.MapLeftKey(new string[] { "RoleId" });
				x.MapRightKey(new string[] { "UserId" });
			});
		}
	}
}