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
    public class BrandConfiguration : EntityTypeConfiguration<Brand>
    {
        public BrandConfiguration()
        {
            base.ToTable("Brand");
            base.HasKey<int>((Brand x) => x.Id).Property<int>((Brand x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
        }
    }
}