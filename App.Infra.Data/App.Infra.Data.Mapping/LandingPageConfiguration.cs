using App.Core.Common;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Other;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
	public class LandingPageConfiguration : EntityTypeConfiguration<LandingPage>
	{
		public LandingPageConfiguration()
		{
			base.ToTable("LandingPage");
			base.HasKey<int>((LandingPage x) => x.Id).Property<int>((LandingPage x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            base.HasRequired<ContactInformation>((LandingPage x) => x.ContactInformation)
                .WithMany((ContactInformation x) => x.LandingPages)
                .HasForeignKey<int>((LandingPage x) => x.ShopId);
		}
	}
}