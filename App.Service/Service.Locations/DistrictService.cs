using App.Core.Utils;
using App.Domain.Entities.Location;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Locations;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Service.Locations
{
	public class DistrictService : BaseService<District>, IDistrictService, IBaseService<District>, IService
	{
		private readonly IDistrictRepository _districtRepository;

		private readonly IUnitOfWork _unitOfWork;

		public DistrictService(IUnitOfWork unitOfWork, IDistrictRepository districtRepository) : base(unitOfWork, districtRepository)
		{
			this._unitOfWork = unitOfWork;
			this._districtRepository = districtRepository;
		}

		public District GetById(int Id)
		{
			return this._districtRepository.GetById(Id);
		}

		public IEnumerable<District> GetByProvinceId(int provinceId)
		{
			IEnumerable<District> districts = this._districtRepository.FindBy((District x) => x.ProvinceId == provinceId, false);
			return districts;
		}

		public IEnumerable<District> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._districtRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}