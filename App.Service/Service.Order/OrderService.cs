using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Order;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Order
{
	public class OrderService : BaseService<App.Domain.Entities.Data.Order>, IOrderService, IBaseService<App.Domain.Entities.Data.Order>, IService
	{
		private readonly IOrderRepository _orderRepository;

		private readonly IUnitOfWork _unitOfWork;

		public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository) : base(unitOfWork, orderRepository)
		{
			this._unitOfWork = unitOfWork;
			this._orderRepository = orderRepository;
		}

		public IEnumerable<App.Domain.Entities.Data.Order> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._orderRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}