using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.FakeEntity.Order;
using App.Framework.Ultis;
using App.ImagePlugin;
using App.Service.Attribute;
using App.Service.Brandes;
using App.Service.Menu;
using App.Service.Order;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class OrderController : BaseAdminUploadController
    {
        private readonly IBrandService _brandService;

        private readonly IOrderGalleryService _galleryService;

        private readonly IImagePlugin _imagePlugin;

        private readonly IMenuLinkService _menuLinkService;

        private readonly IOrderService _orderService;

        private readonly IOrderItemService _orderItemService;

        public OrderController(IOrderService orderService, IMenuLinkService menuLinkService, IAttributeValueService attributeValueService, IOrderGalleryService galleryService, IImagePlugin imagePlugin, IBrandService brandService, IOrderItemService orderItemService)
        {
            this._orderService = orderService;
            this._menuLinkService = menuLinkService;
            this._galleryService = galleryService;
            this._imagePlugin = imagePlugin;
            this._brandService = brandService;
            this._orderItemService = orderItemService;
        }

        [RequiredPermisson(Roles = "CreateEditOrder")]
        public ActionResult Create()
        {
            int id = 1;
            Order order = this._orderService.GetTop<int>(1, (Order x) => x.OrderCode != null, (Order x) => x.Id).FirstOrDefault<Order>();
            if (order != null)
            {
                id = order.Id;
            }
            return base.View(new OrderViewModel()
            {
                OrderCode = string.Concat("DH", id.ToString()),
                CustomerCode = string.Concat("KH", id.ToString())
            });
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditOrder")]
        [ValidateInput(false)]
        public ActionResult Create([Bind] OrderViewModel order, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(order);
                }
                else
                {
                    HttpFileCollectionBase files = base.Request.Files;
                    List<OrderGallery> orderGalleries = new List<OrderGallery>();
                    if (files.Count > 0)
                    {
                        int count = files.Count - 1;
                        int num = 0;
                        string[] allKeys = files.AllKeys;
                        for (int i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str = allKeys[i];
                            if (num <= count)
                            {
                                if (!str.Equals("Image"))
                                {
                                    HttpPostedFileBase httpPostedFileBase = files[num];
                                    if (httpPostedFileBase.ContentLength > 0)
                                    {
                                        OrderGalleryViewModel orderGalleryViewModel = new OrderGalleryViewModel()
                                        {
                                            OrderId = order.Id
                                        };
                                        string str1 = string.Format("{0}-{1}.jpg", order.OrderCode, Guid.NewGuid());
                                        this._imagePlugin.CropAndResizeImage(httpPostedFileBase, string.Format("{0}{1}/", Contains.ImageFolder, order.OrderCode), str1, new int?(ImageSize.WithBigSize), new int?(ImageSize.WithBigSize), false);
                                        orderGalleryViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.ImageFolder, order.OrderCode, str1);
                                        orderGalleries.Add(Mapper.Map<OrderGallery>(orderGalleryViewModel));
                                    }
                                    num++;
                                }
                                else
                                {
                                    num++;
                                }
                            }
                        }
                    }
                    Order order1 = Mapper.Map<OrderViewModel, Order>(order);
                    if (orderGalleries.IsAny<OrderGallery>())
                    {
                        order1.OrderGalleries = orderGalleries;
                    }
                    List<OrderItem> orderItems = new List<OrderItem>();
                    if (order.OrderItems.IsAny<OrderItemViewModel>())
                    {
                        orderItems.AddRange(
                            from item in order.OrderItems
                            select Mapper.Map<OrderItem>(item));
                    }
                    if (orderItems.IsAny<OrderItem>())
                    {
                        order1.OrderItems = orderItems;
                    }
                    this._orderService.Create(order1);
                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Order)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Order.Create: ", exception.Message));
                base.ModelState.AddModelError("", exception.Message);
                return base.View(order);
            }
            return action;
        }

        [RequiredPermisson(Roles = "DeleteOrder")]
        public ActionResult Delete(string[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    List<Order> orders = new List<Order>();
                    List<OrderGallery> orderGalleries = new List<OrderGallery>();
                    string[] strArrays = ids;
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        int num = int.Parse(strArrays[i]);
                        Order order = this._orderService.Get((Order x) => x.Id == num, false);
                        orderGalleries.AddRange(order.OrderGalleries.ToList<OrderGallery>());
                        orders.Add(order);
                    }
                    this._galleryService.BatchDelete(orderGalleries);
                    this._orderService.BatchDelete(orders);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Order.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditOrder")]
        public ActionResult DeleteGallery(int OrderId, int galleryId)
        {
            ActionResult actionResult;
            if (!base.Request.IsAjaxRequest())
            {
                return base.Json(new { success = false });
            }
            try
            {
                OrderGallery orderGallery = this._galleryService.Get((OrderGallery x) => x.OrderId == OrderId && x.Id == galleryId, false);
                this._galleryService.Delete(orderGallery);
                string str = base.Server.MapPath(string.Concat("~/", orderGallery.ImagePath));
                string str1 = base.Server.MapPath(string.Concat("~/", orderGallery.ImagePath));
                System.IO.File.Delete(str);
                System.IO.File.Delete(str1);
                actionResult = base.Json(new { success = true });
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                actionResult = base.Json(new { success = false, messages = exception.Message });
            }
            return actionResult;
        }

        [RequiredPermisson(Roles = "CreateEditOrder")]
        public ActionResult Edit(int Id)
        {
            OrderViewModel orderViewModel = Mapper.Map<Order, OrderViewModel>(this._orderService.Get((Order x) => x.Id == Id, false));
            return base.View(orderViewModel);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditOrder")]
        [ValidateInput(false)]
        public ActionResult Edit([Bind] OrderViewModel orderView, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(orderView);
                }
                else
                {
                    Order order = this._orderService.Get((Order x) => x.Id == orderView.Id, false);
                    HttpFileCollectionBase files = base.Request.Files;
                    List<OrderGallery> orderGalleries = new List<OrderGallery>();
                    if (files.Count > 0)
                    {
                        int count = files.Count - 1;
                        int num = 0;
                        string[] allKeys = files.AllKeys;
                        for (int i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str = allKeys[i];
                            if (num <= count)
                            {
                                if (!str.Equals("Image"))
                                {
                                    HttpPostedFileBase item = files[num];
                                    if (item.ContentLength > 0)
                                    {
                                        OrderGalleryViewModel orderGalleryViewModel = new OrderGalleryViewModel()
                                        {
                                            OrderId = orderView.Id
                                        };
                                        string str1 = string.Format("{0}-{1}.jpg", orderView.OrderCode, Guid.NewGuid());
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.ImageFolder, orderView.OrderCode), str1, new int?(ImageSize.WithBigSize), new int?(ImageSize.WithBigSize), false);
                                        orderGalleryViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.ImageFolder, orderView.OrderCode, str1);
                                        orderGalleries.Add(Mapper.Map<OrderGallery>(orderGalleryViewModel));
                                    }
                                    num++;
                                }
                                else
                                {
                                    num++;
                                }
                            }
                        }
                    }
                    if (orderGalleries.IsAny<OrderGallery>())
                    {
                        order.OrderGalleries = orderGalleries;
                    }
                    List<OrderItem> orderItems = new List<OrderItem>();
                    if (orderView.OrderItems.IsAny<OrderItemViewModel>())
                    {
                        foreach (OrderItemViewModel orderItem in orderView.OrderItems)
                        {
                            OrderItemViewModel orderItemViewModel = new OrderItemViewModel();
                            if (orderItem.Id > 0)
                            {
                                orderItemViewModel.Id = orderItem.Id;
                            }
                            orderItemViewModel.OrderId = orderView.Id;
                            orderItemViewModel.FixedFee = orderItem.FixedFee;
                            orderItemViewModel.Category = orderItem.Category;
                            orderItemViewModel.WarrantyFrom = orderItem.WarrantyFrom;
                            orderItemViewModel.WarrantyTo = orderItem.WarrantyTo;
                            orderItems.Add(Mapper.Map<OrderItem>(orderItemViewModel));
                        }
                    }
                    if (orderItems.IsAny<OrderItem>())
                    {
                        order.OrderItems = orderItems;
                    }
                    IEnumerable<OrderItem> orderItems1 = this._orderItemService.FindBy((OrderItem x) => x.OrderId == orderView.Id, false);
                    this._orderItemService.BatchDelete(orderItems1);
                    order = Mapper.Map<OrderViewModel, Order>(orderView, order);
                    this._orderService.Update(order);
                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Order)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                base.ModelState.AddModelError("", exception.Message);
                ExtentionUtils.Log(string.Concat("Order.Edit: ", exception.Message));
                return base.View(orderView);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewOrder")]
        public ActionResult Index(int page = 1, string keywords = "")
        {
            ((dynamic)base.ViewBag).Keywords = keywords;
            SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
            {
                Keywords = keywords,
                Sorts = new SortBuilder()
                {
                    ColumnName = "CreatedDate",
                    ColumnOrder = SortBuilder.SortOrder.Descending
                }
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            IEnumerable<Order> orders = this._orderService.PagedList(sortingPagingBuilder, paging);
            if (orders != null && orders.Any<Order>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(orders);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("edit") || filterContext.RouteData.Values["action"].Equals("create"))
            {
                dynamic viewBag = base.ViewBag;
                IBrandService brandService = this._brandService;
                viewBag.Brands = brandService.FindBy((Brand x) => x.Status == 1, false);
            }
        }

        public ActionResult WarrantyForm()
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            orderViewModel.OrderItems.Add(new OrderItemViewModel());
            return base.View(orderViewModel);
        }
    }
}