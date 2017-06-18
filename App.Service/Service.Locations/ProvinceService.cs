using App.Core.Utils;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Locations;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Locations
{
	public class ProvinceService : BaseService<Province>, IProvinceService, IBaseService<Province>, IService
	{
		private readonly IProvinceRepository _provinceRepository;

		private readonly IUnitOfWork _unitOfWork;

		public ProvinceService(IUnitOfWork unitOfWork, IProvinceRepository provinceRepository) : base(unitOfWork, provinceRepository)
		{
			this._unitOfWork = unitOfWork;
			this._provinceRepository = provinceRepository;
		}

		public Province GetById(int Id)
		{
			return this._provinceRepository.GetById(Id);
		}

		public IEnumerable<Province> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._provinceRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}