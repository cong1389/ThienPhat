using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Attribute;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Attribute
{
	public class AttributeValueService : BaseService<AttributeValue>, IAttributeValueService, IBaseService<AttributeValue>, IService
	{
		private readonly IAttributeValueRepository _attributeValueRepository;

		private readonly IUnitOfWork _unitOfWork;

		public AttributeValueService(IUnitOfWork unitOfWork, IAttributeValueRepository attributeValueRepository) : base(unitOfWork, attributeValueRepository)
		{
			this._unitOfWork = unitOfWork;
			this._attributeValueRepository = attributeValueRepository;
		}

		public AttributeValue GetById(int Id)
		{
			return this._attributeValueRepository.GetById(Id);
		}

		public IEnumerable<AttributeValue> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._attributeValueRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}