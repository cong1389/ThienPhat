using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Gallery
{
	public class GalleryRepository : RepositoryBase<GalleryImage>, IGalleryRepository, IRepositoryBase<GalleryImage>
	{
		public GalleryRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<GalleryImage> GetDefaultOrder(IQueryable<GalleryImage> query)
		{
			IOrderedQueryable<GalleryImage> galleryImages = 
				from p in query
				orderby p.Id
				select p;
			return galleryImages;
		}

		public GalleryImage GetGalleryById(int id)
		{
			GalleryImage galleryImage = this.FindBy((GalleryImage x) => x.Id == id, false).FirstOrDefault<GalleryImage>();
			return galleryImage;
		}
	}
}