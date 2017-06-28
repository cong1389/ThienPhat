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
	public class NewsConfiguration : EntityTypeConfiguration<News>
	{
		public NewsConfiguration()
		{
			base.ToTable("News");
			base.HasKey<int>((News x) => x.Id).Property<int>((News x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
			base.HasRequired<MenuLink>((News x) => x.MenuLink)
                .WithMany((MenuLink x) => x.News)
                .HasForeignKey<int>((News x) => x.MenuId);
		}
	}
}