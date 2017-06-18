using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Language
{
	public class LanguageRepository : RepositoryBase<App.Domain.Entities.Language.Language>, ILanguageRepository, IRepositoryBase<App.Domain.Entities.Language.Language>
	{
		public LanguageRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<App.Domain.Entities.Language.Language> GetDefaultOrder(IQueryable<App.Domain.Entities.Language.Language> query)
		{
			IOrderedQueryable<App.Domain.Entities.Language.Language> languages = 
				from p in query
				orderby p.Id
				select p;
			return languages;
		}

		public App.Domain.Entities.Language.Language GetLanguageById(int id)
		{
			App.Domain.Entities.Language.Language language = this.FindBy((App.Domain.Entities.Language.Language x) => x.Id == id, false).FirstOrDefault<App.Domain.Entities.Language.Language>();
			return language;
		}

		public IEnumerable<App.Domain.Entities.Language.Language> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<App.Domain.Entities.Language.Language>();
		}

		public IEnumerable<App.Domain.Entities.Language.Language> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Language.Language, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Language.Language>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Language.Language>((App.Domain.Entities.Language.Language x) => x.LanguageCode.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.LanguageName.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}