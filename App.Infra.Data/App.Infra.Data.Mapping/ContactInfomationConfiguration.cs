using App.Core.Common;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Location;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
	public class ContactInfomationConfiguration : EntityTypeConfiguration<ContactInfomation>
	{
		public ContactInfomationConfiguration()
		{
			base.ToTable("ContactInfomation");
			base.HasKey<int>((ContactInfomation x) => x.Id).Property<int>((ContactInfomation x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
			base.HasOptional<Province>((ContactInfomation x) => x.Province).WithMany((Province x) => x.ContactInfomations).HasForeignKey<int?>((ContactInfomation x) => x.ProvinceId);
		}
	}
}