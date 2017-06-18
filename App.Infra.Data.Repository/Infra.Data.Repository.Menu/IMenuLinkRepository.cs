using App.Core.Utils;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Menu
{
	public interface IMenuLinkRepository : IRepositoryBase<MenuLink>
	{
		MenuLink GetById(int Id);

		IEnumerable<MenuLink> PagedList(Paging page);

		IEnumerable<MenuLink> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}