using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
	public class StaticContentConfiguration : EntityTypeConfiguration<StaticContent>
	{
		public StaticContentConfiguration()
		{
			base.ToTable("StaticContents");
			base.HasKey<int>((StaticContent x) => x.Id).Property<int>((StaticContent x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
			base.HasRequired<MenuLink>((StaticContent x) => x.MenuLink).WithMany((MenuLink x) => x.StaticContents).HasForeignKey<int>((StaticContent x) => x.MenuId);
		}
	}
}