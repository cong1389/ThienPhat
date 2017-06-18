using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System;

namespace App.Service.Gallery
{
	public interface IGalleryService : IBaseService<GalleryImage>, IService
	{
		GalleryImage GetGalleryById(int Id);
	}
}