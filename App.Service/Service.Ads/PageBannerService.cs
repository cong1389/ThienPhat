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
	public class PageBannerService : BaseService<PageBanner>, IPageBannerService, IBaseService<PageBanner>, IService
	{
		private readonly IPageBannerRepository _pageBannerRepository;

		private readonly IUnitOfWork _unitOfWork;

		public PageBannerService(IUnitOfWork unitOfWork, IPageBannerRepository pageBannerRepository) : base(unitOfWork, pageBannerRepository)
		{
			this._unitOfWork = unitOfWork;
			this._pageBannerRepository = pageBannerRepository;
		}

		public PageBanner GetById(int Id)
		{
			return this._pageBannerRepository.GetById(Id);
		}

		public IEnumerable<PageBanner> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._pageBannerRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}