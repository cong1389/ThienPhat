using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.LocaleStringResource
{
	public interface ILocaleStringResourceRepository : IRepositoryBase<App.Domain.Entities.Language.LocaleStringResource>
	{
		App.Domain.Entities.Language.LocaleStringResource GetLocaleStringResourceById(int Id);

		IEnumerable<App.Domain.Entities.Language.LocaleStringResource> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.Language.LocaleStringResource> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}