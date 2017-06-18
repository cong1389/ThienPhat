using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Language
{
	public interface ILanguageRepository : IRepositoryBase<App.Domain.Entities.Language.Language>
	{
		App.Domain.Entities.Language.Language GetLanguageById(int id);

		IEnumerable<App.Domain.Entities.Language.Language> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.Language.Language> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}