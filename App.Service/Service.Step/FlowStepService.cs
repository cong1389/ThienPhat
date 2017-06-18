using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Step;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Step
{
	public class FlowStepService : BaseService<FlowStep>, IFlowStepService, IBaseService<FlowStep>, IService
	{
		private readonly IFlowStepRepository _flowStepRepository;

		private readonly IUnitOfWork _unitOfWork;

		public FlowStepService(IUnitOfWork unitOfWork, IFlowStepRepository flowStepRepository) : base(unitOfWork, flowStepRepository)
		{
			this._unitOfWork = unitOfWork;
			this._flowStepRepository = flowStepRepository;
		}

		public IEnumerable<FlowStep> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._flowStepRepository.PagedSearchList(sortbuBuilder, page);
		}

		public IEnumerable<FlowStep> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			return this._flowStepRepository.PagedSearchListByMenu(sortBuider, page);
		}
	}
}