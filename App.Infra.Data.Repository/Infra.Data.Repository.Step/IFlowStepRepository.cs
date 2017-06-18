using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Step
{
	public interface IFlowStepRepository : IRepositoryBase<FlowStep>
	{
		IEnumerable<FlowStep> PagedList(Paging page);

		IEnumerable<FlowStep> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<FlowStep> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}