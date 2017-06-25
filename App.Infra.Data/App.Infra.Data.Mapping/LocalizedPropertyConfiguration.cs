using App.Domain.Entities.Language;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace App.Infra.Data.Mapping
{
    public class LocalizedPropertyConfiguration : EntityTypeConfiguration<LocalizedProperty>
	{
		public LocalizedPropertyConfiguration()
		{
			base.ToTable("LocalizedProperty");
			base.HasKey<int>((LocalizedProperty x) => x.Id).Property<int>((LocalizedProperty x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
		}
	}
}