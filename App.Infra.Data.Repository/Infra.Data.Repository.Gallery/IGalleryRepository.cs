using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System;

namespace App.Infra.Data.Repository.Gallery
{
	public interface IGalleryRepository : IRepositoryBase<GalleryImage>
	{
		GalleryImage GetGalleryById(int id);
	}
}