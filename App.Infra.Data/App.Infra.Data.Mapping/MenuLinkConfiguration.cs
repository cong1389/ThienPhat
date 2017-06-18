using App.Core.Common;
using App.Domain.Entities.Menu;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
	public class MenuLinkConfiguration : EntityTypeConfiguration<MenuLink>
	{
		public MenuLinkConfiguration()
		{
			base.ToTable("MenuLink");
			base.HasKey<int>((MenuLink x) => x.Id).Property<int>((MenuLink x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
			base.HasOptional<MenuLink>((MenuLink e) => e.ParentMenu).WithMany().HasForeignKey<int?>((MenuLink m) => m.ParentId);
		}
	}
}