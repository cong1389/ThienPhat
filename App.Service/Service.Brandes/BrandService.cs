using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Brandes;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Brandes
{
	public class BrandService : BaseService<Brand>, IBrandService, IBaseService<Brand>, IService
	{
		private readonly IBrandRepository _BrandRepository;

		private readonly IUnitOfWork _unitOfWork;

		public BrandService(IUnitOfWork unitOfWork, IBrandRepository BrandRepository) : base(unitOfWork, BrandRepository)
		{
			this._unitOfWork = unitOfWork;
			this._BrandRepository = BrandRepository;
		}

		public Brand GetById(int Id)
		{
			return this._BrandRepository.GetById(Id);
		}

		public IEnumerable<Brand> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._BrandRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}