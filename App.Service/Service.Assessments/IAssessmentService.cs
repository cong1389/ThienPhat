using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.Assessments
{
	public interface IAssessmentService : IBaseService<Assessment>, IService
	{
		IEnumerable<Assessment> PagedList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<Assessment> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}