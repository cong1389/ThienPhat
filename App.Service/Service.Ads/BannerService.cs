using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Ads;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Ads
{
	public class BannerService : BaseService<Banner>, IBannerService, IBaseService<Banner>, IService
	{
		private readonly IBannerRepository _bannerRepository;

		private readonly IUnitOfWork _unitOfWork;

		public BannerService(IUnitOfWork unitOfWork, IBannerRepository bannerRepository) : base(unitOfWork, bannerRepository)
		{
			this._unitOfWork = unitOfWork;
			this._bannerRepository = bannerRepository;
		}

		public Banner GetById(int Id)
		{
			return this._bannerRepository.GetById(Id);
		}

		public IEnumerable<Banner> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._bannerRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}