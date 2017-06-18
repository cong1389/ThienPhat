using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Language
{
	public interface ILanguageService : IBaseService<App.Domain.Entities.Language.Language>, IService
	{
		void CreateLanguage(App.Domain.Entities.Language.Language language);

		App.Domain.Entities.Language.Language GetLanguageById(int Id);

		IEnumerable<App.Domain.Entities.Language.Language> PagedList(SortingPagingBuilder sortBuider, Paging page);

		int SaveLanguage();
	}
}