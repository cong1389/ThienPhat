using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.Step
{
	public interface IFlowStepService : IBaseService<FlowStep>, IService
	{
		IEnumerable<FlowStep> PagedList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<FlowStep> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}