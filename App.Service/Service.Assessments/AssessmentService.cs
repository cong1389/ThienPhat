using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Assessments;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Assessments
{
	public class AssessmentService : BaseService<Assessment>, IAssessmentService, IBaseService<Assessment>, IService
	{
		private readonly IAssessmentRepository _assessmentRepository;

		private readonly IUnitOfWork _unitOfWork;

		public AssessmentService(IUnitOfWork unitOfWork, IAssessmentRepository assessmentRepository) : base(unitOfWork, assessmentRepository)
		{
			this._unitOfWork = unitOfWork;
			this._assessmentRepository = assessmentRepository;
		}

		public IEnumerable<Assessment> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._assessmentRepository.PagedSearchList(sortbuBuilder, page);
		}

		public IEnumerable<Assessment> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			return this._assessmentRepository.PagedSearchListByMenu(sortBuider, page);
		}
	}
}