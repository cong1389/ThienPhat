using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Step
{
	public class FlowStepRepository : RepositoryBase<FlowStep>, IFlowStepRepository, IRepositoryBase<FlowStep>
	{
		public FlowStepRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<FlowStep> GetDefaultOrder(IQueryable<FlowStep> query)
		{
			IOrderedQueryable<FlowStep> flowSteps = 
				from p in query
				orderby p.Id
				select p;
			return flowSteps;
		}

		public IEnumerable<FlowStep> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<FlowStep>();
		}

		public IEnumerable<FlowStep> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<FlowStep, bool>> expression = PredicateBuilder.True<FlowStep>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<FlowStep>((FlowStep x) => x.Title.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}

		public IEnumerable<FlowStep> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<FlowStep, bool>> expression = PredicateBuilder.True<FlowStep>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<FlowStep>((FlowStep x) => x.Title.Contains(sortBuider.Keywords) && x.Status == 1);
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}