using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Post
{
	public interface IPostService : IBaseService<App.Domain.Entities.Data.Post>, IService
	{
		App.Domain.Entities.Data.Post GetById(int Id);

		IEnumerable<App.Domain.Entities.Data.Post> GetBySeoUrl(string seoUrl);

		IEnumerable<App.Domain.Entities.Data.Post> PagedList(SortingPagingBuilder sortBuider, Paging page);

		IEnumerable<App.Domain.Entities.Data.Post> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page);
	}
}