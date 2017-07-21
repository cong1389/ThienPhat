using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace App.Infra.Data.Mapping
{
    public class LocaleStringResourceConfiguration : EntityTypeConfiguration<LocaleStringResource>
	{
		public LocaleStringResourceConfiguration()
		{
			base.ToTable("LocaleStringResource");
			base.HasKey<int>((LocaleStringResource x) => x.Id)
                .Property<int>((LocaleStringResource x) => x.Id)
                .HasColumnName("Id").HasColumnType("int")
                .HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
            
        }
	}
}