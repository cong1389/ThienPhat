using App.Domain.Entities.Account;
using System;
using System.Data.Entity.ModelConfiguration;
namespace App.Infra.Data.Mapping
{
    public class ExternalLoginConfiguration : EntityTypeConfiguration<ExternalLogin>
    {
        public ExternalLoginConfiguration()
        {
            base.ToTable("ExternalLogin");
            base.HasKey((ExternalLogin x) => new
            {
                x.LoginProvider,
                x.ProviderKey,
                x.UserId
            });
            base.Property((ExternalLogin x) => x.LoginProvider).HasColumnName("LoginProvider").HasColumnType("nvarchar").HasMaxLength(new int?(128)).IsRequired();
            base.Property((ExternalLogin x) => x.ProviderKey).HasColumnName("ProviderKey").HasColumnType("nvarchar").HasMaxLength(new int?(128)).IsRequired();
            base.Property<Guid>((ExternalLogin x) => x.UserId).HasColumnName("UserId").HasColumnType("uniqueidentifier").IsRequired();

            base.HasRequired<User>((ExternalLogin x) => x.User)
                .WithMany((User x) => x.Logins)
                .HasForeignKey<Guid>((ExternalLogin x) => x.UserId);
        }
    }
}
