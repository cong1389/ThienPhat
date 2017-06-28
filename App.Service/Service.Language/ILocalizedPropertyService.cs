using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.LocalizedProperty
{
	public interface ILocalizedPropertyService : IBaseService<App.Domain.Entities.Language.LocalizedProperty>, IService
	{
		void CreateLocalizedProperty(App.Domain.Entities.Language.LocalizedProperty LocalizedProperty);

		App.Domain.Entities.Language.LocalizedProperty GetLocalizedPropertyById(int Id);

        IEnumerable<App.Domain.Entities.Language.LocalizedProperty> GetLocalizedPropertyByEntityId(int entityId);

        IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedList(SortingPagingBuilder sortBuider, Paging page);

		int SaveLocalizedProperty();
	}
}