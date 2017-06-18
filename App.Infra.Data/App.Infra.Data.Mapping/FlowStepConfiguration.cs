using App.Core.Common;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
	public class FlowStepConfiguration : EntityTypeConfiguration<FlowStep>
	{
		public FlowStepConfiguration()
		{
			base.ToTable("FlowStep");
			base.HasKey<int>((FlowStep x) => x.Id).Property<int>((FlowStep x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
		}
	}
}