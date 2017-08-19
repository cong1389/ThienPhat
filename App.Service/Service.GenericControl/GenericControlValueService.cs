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
	public class GenericControlValueService : BaseService<GenericControlValue>, IGenericControlValueService, IBaseService<GenericControlValue>, IService
	{
		private readonly IGenericControlValueRepository _attributeValueRepository;

		private readonly IUnitOfWork _unitOfWork;

		public GenericControlValueService(IUnitOfWork unitOfWork, IGenericControlValueRepository attributeValueRepository) : base(unitOfWork, attributeValueRepository)
		{
			this._unitOfWork = unitOfWork;
			this._attributeValueRepository = attributeValueRepository;
		}

		public GenericControlValue GetById(int Id)
		{
			return this._attributeValueRepository.GetById(Id);
		}

		public IEnumerable<GenericControlValue> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._attributeValueRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}