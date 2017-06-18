using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Order;
using App.Infra.Data.UOW.Interfaces;
using System;

namespace App.Service.Order
{
	public class OrderGalleryService : BaseService<OrderGallery>, IOrderGalleryService, IBaseService<OrderGallery>, IService
	{
		private readonly IOrderGalleryRepository _galleryRepository;

		private readonly IUnitOfWork _unitOfWork;

		public OrderGalleryService(IUnitOfWork unitOfWork, IOrderGalleryRepository galleryRepository) : base(unitOfWork, galleryRepository)
		{
			this._unitOfWork = unitOfWork;
			this._galleryRepository = galleryRepository;
		}
	}
}