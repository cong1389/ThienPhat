using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Language
{
	public interface ILocalizedPropertyRepository : IRepositoryBase<App.Domain.Entities.Language.LocalizedProperty>
	{
		App.Domain.Entities.Language.LocalizedProperty GetId(int id);

		IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}