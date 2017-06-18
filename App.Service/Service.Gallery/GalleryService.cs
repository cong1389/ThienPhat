using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Gallery;
using App.Infra.Data.UOW.Interfaces;
using System;

namespace App.Service.Gallery
{
	public class GalleryService : BaseService<GalleryImage>, IGalleryService, IBaseService<GalleryImage>, IService
	{
		private readonly IGalleryRepository _galleryRepository;

		private readonly IUnitOfWork _unitOfWork;

		public GalleryService(IUnitOfWork unitOfWork, IGalleryRepository galleryRepository) : base(unitOfWork, galleryRepository)
		{
			this._unitOfWork = unitOfWork;
			this._galleryRepository = galleryRepository;
		}

		public GalleryImage GetGalleryById(int Id)
		{
			return this._galleryRepository.GetGalleryById(Id);
		}
	}
}