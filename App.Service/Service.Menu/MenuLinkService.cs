using App.Core.Utils;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Menu;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Service.Menu
{
	public class MenuLinkService : BaseService<MenuLink>, IMenuLinkService, IBaseService<MenuLink>, IService
	{
		private readonly IMenuLinkRepository _menuLinkRepository;

		private readonly IUnitOfWork _unitOfWork;

		public MenuLinkService(IUnitOfWork unitOfWork, IMenuLinkRepository menuLinkRepository) : base(unitOfWork, menuLinkRepository)
		{
			this._unitOfWork = unitOfWork;
			this._menuLinkRepository = menuLinkRepository;
		}

		public MenuLink GetById(int Id)
		{
			return this._menuLinkRepository.GetById(Id);
		}

		public IEnumerable<MenuLink> GetBySeoUrl(string seoUrl)
		{
			IEnumerable<MenuLink> menuLinks = this._menuLinkRepository.FindBy((MenuLink x) => x.SeoUrl.Equals(seoUrl), false);
			return menuLinks;
		}

		public IEnumerable<MenuLink> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._menuLinkRepository.PagedSearchList(sortbuBuilder, page);
		}

		public int Save()
		{
			return this._unitOfWork.Commit();
		}
	}
}