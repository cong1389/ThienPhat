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
	public class AttributeService : BaseService<App.Domain.Entities.Attribute.Attribute>, IAttributeService, IBaseService<App.Domain.Entities.Attribute.Attribute>, IService
	{
		private readonly IAttributeRepository _attributeRepository;

		private readonly IUnitOfWork _unitOfWork;

		public AttributeService(IUnitOfWork unitOfWork, IAttributeRepository attributeRepository) : base(unitOfWork, attributeRepository)
		{
			this._unitOfWork = unitOfWork;
			this._attributeRepository = attributeRepository;
		}

		public App.Domain.Entities.Attribute.Attribute GetById(int Id)
		{
			return this._attributeRepository.GetById(Id);
		}

		public IEnumerable<App.Domain.Entities.Attribute.Attribute> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._attributeRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}