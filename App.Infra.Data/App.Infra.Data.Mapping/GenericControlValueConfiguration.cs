using App.Core.Common;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using App.Domain.Entities.GlobalSetting;

namespace App.Infra.Data.Mapping
{
	public class GenericControlValueConfiguration : EntityTypeConfiguration<GenericControlValue>
	{
		public GenericControlValueConfiguration()
		{
            base.ToTable("GenericControlValue");

            base.HasKey<int>((GenericControlValue x) => x.Id).Property<int>((GenericControlValue x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            base.HasRequired<GenericControl>((GenericControlValue x) => x.GenericControl)
                           .WithMany((GenericControl x) => x.GenericControlValues)
                           .HasForeignKey<int>((GenericControlValue x) => x.GenericControlId);

        }
    }
}