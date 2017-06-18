using App.Core.Utils;
using App.Domain.Entities.Slide;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.Slide
{
    public interface ISlideShowService : IBaseService<SlideShow>, IService
    {
        IEnumerable<SlideShow> PagedList(SortingPagingBuilder sortBuider, Paging page);
    }
}