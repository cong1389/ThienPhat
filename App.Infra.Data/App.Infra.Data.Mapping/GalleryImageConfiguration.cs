using App.Core.Common;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
	public class GalleryImageConfiguration : EntityTypeConfiguration<GalleryImage>
	{
		public GalleryImageConfiguration()
		{
			base.ToTable("GalleryImage");
			base.HasKey<int>((GalleryImage x) => x.Id).Property<int>((GalleryImage x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
			base.HasRequired<Post>((GalleryImage x) => x.Post).WithMany((Post x) => x.GalleryImages).HasForeignKey<int>((GalleryImage x) => x.PostId).WillCascadeOnDelete(true);
			base.HasRequired<AttributeValue>((GalleryImage x) => x.AttributeValue).WithMany((AttributeValue x) => x.GalleryImages).HasForeignKey<int>((GalleryImage x) => x.AttributeValueId).WillCascadeOnDelete(true);
		}
	}
}