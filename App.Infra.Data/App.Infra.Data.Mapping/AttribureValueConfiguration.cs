using App.Core.Common;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Mapping
{
	public class AttribureValueConfiguration : EntityTypeConfiguration<AttributeValue>
	{
		public AttribureValueConfiguration()
		{
			base.ToTable("AttribureValue");
			base.HasKey<int>((AttributeValue x) => x.Id).Property<int>((AttributeValue x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
			base.HasRequired<App.Domain.Entities.Attribute.Attribute>((AttributeValue x) => x.Attribute).WithMany((App.Domain.Entities.Attribute.Attribute x) => x.AttributeValues).HasForeignKey<int>((AttributeValue x) => x.AttributeId);
			base.HasMany<Post>((AttributeValue x) => x.Posts).WithMany((Post x) => x.AttributeValues).Map((ManyToManyAssociationMappingConfiguration x) => {
				x.ToTable("PostAttribute");
				x.MapLeftKey(new string[] { "AttibuteValueId" });
				x.MapRightKey(new string[] { "PostId" });
			});
		}
	}
}