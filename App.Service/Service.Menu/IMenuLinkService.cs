using App.Core.Utils;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Menu
{
	public interface IMenuLinkService : IBaseService<MenuLink>, IService
	{
		MenuLink GetById(int Id);

		IEnumerable<MenuLink> GetBySeoUrl(string seoUrl);

		IEnumerable<MenuLink> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}