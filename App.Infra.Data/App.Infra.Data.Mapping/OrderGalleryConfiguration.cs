using App.Core.Common;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
    public class OrderGalleryConfiguration : EntityTypeConfiguration<OrderGallery>
    {
        public OrderGalleryConfiguration()
        {
            base.ToTable("OrderGallery");
            base.HasKey<int>((OrderGallery x) => x.Id).Property<int>((OrderGallery x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
            base.HasRequired<Order>((OrderGallery x) => x.Order).WithMany((Order x) => x.OrderGalleries).HasForeignKey<int>((OrderGallery x) => x.OrderId).WillCascadeOnDelete(true);
        }
    }
}