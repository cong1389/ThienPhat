using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Service.Order;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class PrintController : Controller
    {
        private readonly IOrderService _orderService;

        public PrintController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        public ActionResult Bill(int id)
        {
            Order order = this._orderService.Get((Order x) => x.Id == id, false);
            return base.View(order);
        }

        public ActionResult Warranty(int id)
        {
            Order order = this._orderService.Get((Order x) => x.Id == id, false);
            return base.View(order);
        }
    }
}