using App.Core.Common;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            base.ToTable("Order");
            base.HasKey<int>((Order x) => x.Id).Property<int>((Order x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
            base.HasRequired<Brand>((Order x) => x.Brand).WithMany((Brand x) => x.Orders).HasForeignKey<int>((Order x) => x.BrandId).WillCascadeOnDelete(true);
        }
    }
}