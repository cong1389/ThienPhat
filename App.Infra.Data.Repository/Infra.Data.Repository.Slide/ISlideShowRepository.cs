using App.Core.Utils;
using App.Domain.Entities.Slide;
using App.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Slide
{
	public interface ISlideShowRepository : IRepositoryBase<SlideShow>
	{
		IEnumerable<SlideShow> PagedList(Paging page);

		IEnumerable<SlideShow> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}