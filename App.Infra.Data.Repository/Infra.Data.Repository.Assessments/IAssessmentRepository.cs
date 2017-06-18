using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Assessments
{
	public interface IAssessmentRepository : IRepositoryBase<Assessment>
	{
        IEnumerable<Assessment> PagedList(Paging page);

        IEnumerable<Assessment> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);

        IEnumerable<Assessment> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page);
    }
}