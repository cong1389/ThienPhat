using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.Language
{
    public class LocalizedPropertyRepository : RepositoryBase<App.Domain.Entities.Language.LocalizedProperty>, ILocalizedPropertyRepository, IRepositoryBase<App.Domain.Entities.Language.LocalizedProperty>
	{
		public LocalizedPropertyRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<App.Domain.Entities.Language.LocalizedProperty> GetDefaultOrder(IQueryable<App.Domain.Entities.Language.LocalizedProperty> query)
		{
			IOrderedQueryable<App.Domain.Entities.Language.LocalizedProperty> languages = 
				from p in query
				orderby p.Id
				select p;
			return languages;
		}

		public App.Domain.Entities.Language.LocalizedProperty GetId(int id)
		{
			App.Domain.Entities.Language.LocalizedProperty language = this.FindBy((App.Domain.Entities.Language.LocalizedProperty x) => x.Id == id, false).FirstOrDefault<App.Domain.Entities.Language.LocalizedProperty>();
			return language;
		}

		public IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<App.Domain.Entities.Language.LocalizedProperty>();
		}

		public IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Language.LocalizedProperty, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Language.LocalizedProperty>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Language.LocalizedProperty>((App.Domain.Entities.Language.LocalizedProperty x) => x.LocaleKey.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.LocaleKey.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}