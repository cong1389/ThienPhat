using App.Core.Common;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Location;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
	public class ContactInformationConfiguration : EntityTypeConfiguration<ContactInformation>
	{
		public ContactInformationConfiguration()
		{
			base.ToTable("ContactInformation");

            base.HasKey<int>((ContactInformation x) => x.Id).Property<int>((ContactInformation x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            base.HasOptional<Province>((ContactInformation x) => x.Province)
                .WithMany((Province x) => x.ContactInformations).HasForeignKey<int?>
                ((ContactInformation x) => x.ProvinceId);

            base.HasMany<GenericControl>((ContactInformation x) => x.GenericControls)
                .WithRequired((GenericControl x) => x.ContactInfo)
                .HasForeignKey<int>((GenericControl x) => x.EntityId);

        }
    }
}