using App.Core.Common;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Mapping
{
    public class PostsConfiguration : EntityTypeConfiguration<Post>
    {
        public PostsConfiguration()
        {
            base.ToTable("Post");
            base.HasKey<int>((Post x) => x.Id).Property<int>((Post x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            base.HasRequired<MenuLink>((Post x) => x.MenuLink)
                .WithMany((MenuLink x) => x.Posts)
                .HasForeignKey<int>((Post x) => x.MenuId).WillCascadeOnDelete(true);

            base.HasMany<AttributeValue>(
                (Post x) => x.AttributeValues)
                .WithMany((AttributeValue x) => x.Posts)
                .Map((ManyToManyAssociationMappingConfiguration x) =>
                {
                    x.ToTable("PostAttribute");
                    x.MapLeftKey(new string[] { "PostId" });
                    x.MapRightKey(new string[] { "AttibuteValueId" });
                });
        }
    }
}