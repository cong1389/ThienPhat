using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Order;
using App.Infra.Data.UOW.Interfaces;
using System;

namespace App.Service.Order
{
	public class OrderItemService : BaseService<OrderItem>, IOrderItemService, IBaseService<OrderItem>, IService
	{
		private readonly IOrderItemRepository _orderItemRepository;

		private readonly IUnitOfWork _unitOfWork;

		public OrderItemService(IUnitOfWork unitOfWork, IOrderItemRepository orderItemRepository) : base(unitOfWork, orderItemRepository)
		{
			this._unitOfWork = unitOfWork;
			this._orderItemRepository = orderItemRepository;
		}
	}
}