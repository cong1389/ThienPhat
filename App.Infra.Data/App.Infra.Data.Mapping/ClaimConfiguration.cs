using App.Core.Common;
using App.Domain.Entities.Account;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
	public class ClaimConfiguration : EntityTypeConfiguration<Claim>
	{
		public ClaimConfiguration()
		{
			base.ToTable("Claim");
			base.HasKey<int>((Claim x) => x.Id).Property<int>((Claim x) => x.Id).HasColumnName("ClaimId").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
			base.Property<Guid>((Claim x) => x.UserId).HasColumnName("UserId").HasColumnType("uniqueidentifier").IsRequired();
			base.Property((Claim x) => x.ClaimType).HasColumnName("ClaimType").HasColumnType("nvarchar").IsMaxLength().IsOptional();
			base.Property((Claim x) => x.ClaimValue).HasColumnName("ClaimValue").HasColumnType("nvarchar").IsMaxLength().IsOptional();
			base.HasRequired<User>((Claim x) => x.User).WithMany((User x) => x.Claims).HasForeignKey<Guid>((Claim x) => x.UserId);
		}
	}
}