using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace App.Infra.Data.Mapping
{
    public class GenericAttributeConfiguration : EntityTypeConfiguration<GenericAttribute>
	{
		public GenericAttributeConfiguration()
		{
			base.ToTable("GenericAttribute");
			base.HasKey<int>((GenericAttribute x) => x.Id)
                .Property<int>((GenericAttribute x) => x.Id)
                .HasColumnName("Id").HasColumnType("int")
                .HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
            
        }
	}
}