using App.Core.Common;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
    public class OrderItemConfiguration : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemConfiguration()
        {
            base.ToTable("OrderItem");
            base.HasKey<int>((OrderItem x) => x.Id).Property<int>((OrderItem x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
            base.HasRequired<Order>((OrderItem x) => x.Order).WithMany((Order x) => x.OrderItems).HasForeignKey<int>((OrderItem x) => x.OrderId).WillCascadeOnDelete(true);
        }
    }
}