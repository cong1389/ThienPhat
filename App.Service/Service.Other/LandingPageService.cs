using App.Core.Utils;
using App.Domain.Entities.Other;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Other;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Other
{
	public class LandingPageService : BaseService<LandingPage>, ILandingPageService, IBaseService<LandingPage>, IService
	{
		private readonly ILandingPageRepository _landingPageRepository;

		private readonly IUnitOfWork _unitOfWork;

		public LandingPageService(IUnitOfWork unitOfWork, ILandingPageRepository landingPageRepository) : base(unitOfWork, landingPageRepository)
		{
			this._unitOfWork = unitOfWork;
			this._landingPageRepository = landingPageRepository;
		}

		public IEnumerable<LandingPage> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._landingPageRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}