using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using App.Infra.Data.Repository.Assessments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Step
{
    public class AssessmentRepository : RepositoryBase<Assessment>, IAssessmentRepository, IRepositoryBase<Assessment>
    {
        public AssessmentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        protected override IOrderedQueryable<Assessment> GetDefaultOrder(IQueryable<Assessment> query)
        {
            IOrderedQueryable<Assessment> assessment =
                from p in query
                orderby p.Id
                select p;
            return assessment;
        }

        public IEnumerable<Assessment> PagedList(Paging page)
        {
            return this.GetAllPagedList(page).ToList<Assessment>();
        }

        public IEnumerable<Assessment> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
        {
            Expression<Func<Assessment, bool>> expression = PredicateBuilder.True<Assessment>();
            if (!string.IsNullOrEmpty(sortBuider.Keywords))
            {
                expression = expression.And<Assessment>((Assessment x) => x.FullName.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
            }
            return this.FindAndSort(expression, sortBuider.Sorts, page);
        }

        public IEnumerable<Assessment> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page)
        {
            Expression<Func<Assessment, bool>> expression = PredicateBuilder.True<Assessment>();
            if (!string.IsNullOrEmpty(sortBuider.Keywords))
            {
                expression = expression.And<Assessment>((Assessment x) => x.FullName.Contains(sortBuider.Keywords) && x.Status == 1);
            }
            return this.FindAndSort(expression, sortBuider.Sorts, page);
        }
    }
}