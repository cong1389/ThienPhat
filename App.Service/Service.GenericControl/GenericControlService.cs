using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.GenericControl;
using App.Infra.Data.UOW.Interfaces;
using App.Service.GenericControl;
using System;
using System.Collections.Generic;

namespace App.Service.GenericControl
{
	public class GenericControlService : BaseService<App.Domain.Entities.GenericControl.GenericControl>, IGenericControlService, IBaseService<App.Domain.Entities.GenericControl.GenericControl>, IService
	{
		private readonly IGenericControlRepository _attributeRepository;

		private readonly IUnitOfWork _unitOfWork;

		public GenericControlService(IUnitOfWork unitOfWork, IGenericControlRepository attributeRepository) : base(unitOfWork, attributeRepository)
		{
			this._unitOfWork = unitOfWork;
			this._attributeRepository = attributeRepository;
		}

		public App.Domain.Entities.GenericControl.GenericControl GetById(int Id)
		{
			return this._attributeRepository.GetById(Id);
		}

		public IEnumerable<App.Domain.Entities.GenericControl.GenericControl> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._attributeRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}