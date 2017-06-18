using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.SeoSetting;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.SeoSetting
{
	public class SettingSeoGlobalService : BaseService<SettingSeoGlobal>, ISettingSeoGlobalService, IBaseService<SettingSeoGlobal>, IService
	{
		private readonly ISettingSeoGlobalRepository _SettingSeoGlobalRepository;

		private readonly IUnitOfWork _unitOfWork;

		public SettingSeoGlobalService(IUnitOfWork unitOfWork, ISettingSeoGlobalRepository SettingSeoGlobalRepository) : base(unitOfWork, SettingSeoGlobalRepository)
		{
			this._unitOfWork = unitOfWork;
			this._SettingSeoGlobalRepository = SettingSeoGlobalRepository;
		}

		public SettingSeoGlobal GetById(int Id)
		{
			return this._SettingSeoGlobalRepository.GetById(Id);
		}

		public IEnumerable<SettingSeoGlobal> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._SettingSeoGlobalRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}