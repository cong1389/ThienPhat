using App.Core.Common;
using App.Domain.Entities.Ads;
using App.Domain.Entities.Menu;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Mapping
{
	public class BannerConfiguration : EntityTypeConfiguration<Banner>
	{
		public BannerConfiguration()
		{
			base.ToTable("Banner");
			base.HasKey<int>((Banner x) => x.Id).Property<int>((Banner x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
			base.HasRequired<PageBanner>((Banner x) => x.PageBanner).WithMany((PageBanner x) => x.Banners)
                .HasForeignKey<int>((Banner x) => x.PageId);
			base.HasOptional<MenuLink>((Banner x) => x.MenuLink)
                .WithMany((MenuLink x) => x.Banners)
                .Map((ForeignKeyAssociationMappingConfiguration m) 
                => m.MapKey(new string[] { "MenuId" }));
		}
	}
}