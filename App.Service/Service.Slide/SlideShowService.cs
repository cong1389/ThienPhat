using App.Core.Utils;
using App.Domain.Entities.Slide;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Slide;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Slide
{
    public class SlideShowService : BaseService<SlideShow>, ISlideShowService, IBaseService<SlideShow>, IService
    {
        private readonly ISlideShowRepository _slideShowRepository;

        private readonly IUnitOfWork _unitOfWork;

        public SlideShowService(IUnitOfWork unitOfWork, ISlideShowRepository slideShowRepository) : base(unitOfWork, slideShowRepository)
        {
            this._unitOfWork = unitOfWork;
            this._slideShowRepository = slideShowRepository;
        }

        public IEnumerable<SlideShow> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._slideShowRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}