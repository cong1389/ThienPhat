using App.Core.Common;
using App.Domain.Entities.Account;
using App.Domain.Entities.Ads;
//using App.Domain.Entities.Assessments;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Language;
using App.Domain.Entities.Location;
using App.Domain.Entities.Menu;
using App.Domain.Entities.Other;
using App.Domain.Entities.Slide;
using App.Infra.Data.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infra.Data.Context
{
    public class AppContext : DbContext
	{
		public virtual DbSet<App.Domain.Entities.Attribute.Attribute> Attributes
		{
			get;
			set;
		}

		public virtual DbSet<AttributeValue> AttributeValues
		{
			get;
			set;
		}

		public virtual DbSet<Banner> Banners
		{
			get;
			set;
		}

		public virtual DbSet<ContactInfomation> ContactInfomations
		{
			get;
			set;
		}

		public virtual DbSet<FlowStep> FlowSteps
		{
			get;
			set;
		}

		public virtual DbSet<GalleryImage> GalleryImages
		{
			get;
			set;
		}

		public virtual DbSet<LandingPage> LandingPages
		{
			get;
			set;
		}

		public virtual DbSet<Language> Languages
		{
			get;
			set;
		}

		public virtual IDbSet<ExternalLogin> Logins
		{
			get;
			set;
		}

		public virtual DbSet<MenuLink> MenuLinks
		{
			get;
			set;
		}

		public virtual DbSet<App.Domain.Entities.Data.News> News
		{
			get;
			set;
		}

		public virtual DbSet<PageBanner> PageBanners
		{
			get;
			set;
		}

		public virtual DbSet<Post> Posts
		{
			get;
			set;
		}

		public virtual DbSet<Province> Provinces
		{
			get;
			set;
		}

        public virtual DbSet<Assessment> Assessments
        {
            get;
            set;
        }

        public virtual IDbSet<Role> Roles
		{
			get;
			set;
		}

		public virtual DbSet<ServerMailSetting> ServerMailSettings
		{
			get;
			set;
		}

		public virtual DbSet<SettingSeoGlobal> SettingSeoGlobals
		{
			get;
			set;
		}

		public virtual DbSet<SlideShow> SlideShows
		{
			get;
			set;
		}

		public virtual DbSet<StaticContent> StaticContents
		{
			get;
			set;
		}

		public virtual DbSet<SystemSetting> SystemSettings
		{
			get;
			set;
		}

		public virtual IDbSet<User> Users
		{
			get;
			set;
		}

        public virtual IDbSet<Brand> Brand
        {
            get;
            set;
        }

        public virtual IDbSet<Order> Order
        {
            get;
            set;
        }

        public virtual IDbSet<OrderGallery> OrderGallery
        {
            get;
            set;
        }

        public virtual IDbSet<OrderItem> OrderItem
        {
            get;
            set;
        }

        public virtual IDbSet<LocalizedProperty> LocalizedProperty
        {
            get;
            set;
        }

        public virtual IDbSet<GenericAttribute> GenericAttribute
        {
            get;
            set;
        }

        public virtual IDbSet<LocaleStringResource> LocaleStringResource
        {
            get;
            set;
        }

        public AppContext() : base("AppConnect")
		{
			base.Configuration.LazyLoadingEnabled = true;
			base.Configuration.ProxyCreationEnabled = true;
		}

		public virtual int Commit()
		{
			IEnumerable<DbEntityEntry> dbEntityEntries = 
				from x in base.ChangeTracker.Entries()
				where (!(x.Entity is IAuditableEntity) ? false : (x.State == EntityState.Added ? true : x.State == EntityState.Modified))
				select x;
			foreach (DbEntityEntry dbEntityEntry in dbEntityEntries)
			{
				IAuditableEntity entity = dbEntityEntry.Entity as IAuditableEntity;
				if (entity != null)
				{
					string name = Thread.CurrentPrincipal.Identity.Name;
					DateTime utcNow = DateTime.UtcNow;
					if (dbEntityEntry.State != EntityState.Added)
					{
						base.Entry<IAuditableEntity>(entity).Property<string>((IAuditableEntity x) => x.CreatedBy).IsModified = false;
						base.Entry<IAuditableEntity>(entity).Property<DateTime>((IAuditableEntity x) => x.CreatedDate).IsModified = false;
					}
					else
					{
						entity.CreatedBy = name;
						entity.CreatedDate = utcNow;
					}
					entity.UpdatedBy = name;
					entity.UpdatedDate = new DateTime?(utcNow);
				}
			}
			return this.SaveChanges();
		}

		public virtual Task<int> CommitAsync()
		{
			IEnumerable<DbEntityEntry> dbEntityEntries = 
				from x in base.ChangeTracker.Entries()
				where (!(x.Entity is IAuditableEntity) ? false : (x.State == EntityState.Added ? true : x.State == EntityState.Modified))
				select x;
			foreach (DbEntityEntry dbEntityEntry in dbEntityEntries)
			{
				IAuditableEntity entity = dbEntityEntry.Entity as IAuditableEntity;
				if (entity != null)
				{
					string name = Thread.CurrentPrincipal.Identity.Name;
					DateTime utcNow = DateTime.UtcNow;
					if (dbEntityEntry.State != EntityState.Added)
					{
						base.Entry<IAuditableEntity>(entity).Property<string>((IAuditableEntity x) => x.CreatedBy).IsModified = false;
						base.Entry<IAuditableEntity>(entity).Property<DateTime>((IAuditableEntity x) => x.CreatedDate).IsModified = false;
					}
					else
					{
						entity.CreatedBy = name;
						entity.CreatedDate = utcNow;
					}
					entity.UpdatedBy = name;
					entity.UpdatedDate = new DateTime?(utcNow);
				}
			}
			return this.SaveChangesAsync();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
			modelBuilder.Properties().Where((PropertyInfo x) => x.Name == string.Concat(x.ReflectedType.Name, "Id")).Configure((ConventionPrimitivePropertyConfiguration x) => x.IsKey().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));
			modelBuilder.Configurations.Add<User>(new UserConfiguration());
			modelBuilder.Configurations.Add<Role>(new RoleConfiguration());
            modelBuilder.Configurations.Add<ExternalLogin>(new ExternalLoginConfiguration());
            modelBuilder.Configurations.Add<Claim>(new ClaimConfiguration());
			modelBuilder.Configurations.Add<MenuLink>(new MenuLinkConfiguration());
			modelBuilder.Configurations.Add<Banner>(new BannerConfiguration());
			modelBuilder.Configurations.Add<ContactInfomation>(new ContactInfomationConfiguration());
			modelBuilder.Configurations.Add<LandingPage>(new LandingPageConfiguration());
			modelBuilder.Configurations.Add<Post>(new PostsConfiguration());
			modelBuilder.Configurations.Add<AttributeValue>(new AttribureValueConfiguration());
			modelBuilder.Configurations.Add<App.Domain.Entities.Data.News>(new NewsConfiguration());
			modelBuilder.Configurations.Add<GalleryImage>(new GalleryImageConfiguration());
			modelBuilder.Configurations.Add<StaticContent>(new StaticContentConfiguration());
			modelBuilder.Configurations.Add<FlowStep>(new FlowStepConfiguration());
            modelBuilder.Configurations.Add<Brand>(new BrandConfiguration());
            modelBuilder.Configurations.Add<Order>(new OrderConfiguration());
            modelBuilder.Configurations.Add<OrderGallery>(new OrderGalleryConfiguration());
            modelBuilder.Configurations.Add<OrderItem>(new OrderItemConfiguration());
            modelBuilder.Configurations.Add<LocalizedProperty>(new LocalizedPropertyConfiguration());

            modelBuilder.Configurations.Add<GenericAttribute>(new GenericAttributeConfiguration());
            modelBuilder.Configurations.Add<LocaleStringResource>(new LocaleStringResourceConfiguration());
        }
	}
}