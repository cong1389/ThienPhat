using App.Core.Common;
using App.Domain.Entities.Account;
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Mapping
{
	public class UserConfiguration : EntityTypeConfiguration<User>
	{
		public UserConfiguration()
		{
			base.ToTable("User");
			base.HasKey<Guid>((User x) => x.Id).Property<Guid>((User x) => x.Id).HasColumnName("UserId").HasColumnType("uniqueidentifier").IsRequired();
			base.Property((User x) => x.PasswordHash).HasColumnName("PasswordHash").HasColumnType("nvarchar").IsMaxLength().IsOptional();
			base.Property((User x) => x.SecurityStamp).HasColumnName("SecurityStamp").HasColumnType("nvarchar").IsMaxLength().IsOptional();
			base.Property((User x) => x.UserName).HasColumnName("UserName").HasColumnType("nvarchar").HasMaxLength(new int?(256)).IsRequired();
			base.Property((User x) => x.Email).HasColumnName("Email").HasColumnType("nvarchar").HasMaxLength(new int?(50)).IsOptional();
			base.HasMany<Role>((User x) => x.Roles).WithMany((Role x) => x.Users).Map((ManyToManyAssociationMappingConfiguration x) => {
				x.ToTable("UserRole");
				x.MapLeftKey(new string[] { "UserId" });
				x.MapRightKey(new string[] { "RoleId" });
			});
			base.HasMany<Claim>((User x) => x.Claims).WithRequired((Claim x) => x.User).HasForeignKey<Guid>((Claim x) => x.UserId);

            base.HasMany<ExternalLogin>((User x) => x.Logins)
                .WithRequired((ExternalLogin x) => x.User)
                .HasForeignKey<Guid>((ExternalLogin x) => x.UserId);
		}
	}
}