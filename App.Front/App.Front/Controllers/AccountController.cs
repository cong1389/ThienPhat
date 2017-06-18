using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Identity;
using App.Domain.Entities.Location;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Gallery;
using App.FakeEntity.Post;
using App.FakeEntity.User;
using App.Framework.Ultis;
using App.Front.Models;
using App.ImagePlugin;
using App.Service.Gallery;
using App.Service.Locations;
using App.Service.Menu;
using App.Service.Post;
using App.Utils;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Front.Controllers
{
    [FrontAuthorize]
    public class AccountController : BaseAccessUserController
    {
        private readonly IPostService _postService;

        private readonly IDistrictService _districtService;

        private readonly IMenuLinkService _menuLinkService;

        private readonly IProvinceService _provinceService;

        private readonly IGalleryService _galleryService;

        private readonly IImagePlugin _imagePlugin;

        public AccountController(UserManager<IdentityUser, Guid> userManager, IPostService postService, IGalleryService galleryService, IProvinceService provinceService, IMenuLinkService menuLinkService, IDistrictService districtService, IImagePlugin imagePlugin) : base(userManager)
        {
            this._postService = postService;
            this._galleryService = galleryService;
            this._provinceService = provinceService;
            this._menuLinkService = menuLinkService;
            this._districtService = districtService;
            this._imagePlugin = imagePlugin;
        }

        [HttpGet]
        public ActionResult ChangeInfo()
        {
            RegisterFormViewModel registerFormViewModel = Mapper.Map<RegisterFormViewModel>(this.UserManager.FindByName<IdentityUser, Guid>(base.HttpContext.User.Identity.Name));
            return base.View(registerFormViewModel);
        }

        [HttpPost]
        public ActionResult ChangeInfo(RegisterFormViewModel model)
        {
            return base.View(model);
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            this.UserManager.FindByName<IdentityUser, Guid>(base.HttpContext.User.Identity.Name);
            return base.View(new PostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult CreatePost(PostViewModel post)
        {
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                }
                else
                {
                    string str = post.Title.NonAccent();
                    IEnumerable<Post> bySeoUrl = this._postService.GetBySeoUrl(str);
                    post.SeoUrl = post.Title.NonAccent();
                    if (bySeoUrl.Any<Post>((Post x) => x.Id != post.Id))
                    {
                        PostViewModel postViewModel = post;
                        postViewModel.SeoUrl = string.Concat(postViewModel.SeoUrl, "-", bySeoUrl.Count<Post>());
                    }
                    if (post.Image != null && post.Image.ContentLength > 0)
                    {
                        string str1 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                        string str2 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                        string str3 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                        this._imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}/{1}/", Contains.PostFolder, str), str1, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
                        this._imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}/{1}/", Contains.PostFolder, str), str2, new int?(ImageSize.WithMediumSize), new int?(ImageSize.HeightMediumSize), false);
                        this._imagePlugin.CropAndResizeImage(post.Image, string.Format("{0}/{1}/", Contains.PostFolder, str), str3, new int?(ImageSize.WithSmallSize), new int?(ImageSize.HeightSmallSize), false);
                        post.ImageBigSize = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str1);
                        post.ImageMediumSize = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str2);
                        post.ImageSmallSize = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str3);
                    }
                    int? menuId = post.MenuId;
                    int i = 0;
                    if ((menuId.GetValueOrDefault() > i ? menuId.HasValue : false))
                    {
                        IMenuLinkService menuLinkService = this._menuLinkService;
                        menuId = post.MenuId;
                        MenuLink byId = menuLinkService.GetById(menuId.Value);
                        post.VirtualCatUrl = byId.VirtualSeoUrl;
                        post.VirtualCategoryId = byId.VirtualId;
                    }
                    HttpFileCollectionBase files = base.Request.Files;
                    List<GalleryImage> galleryImages = null;
                    if (files.Count > 0)
                    {
                        galleryImages = new List<GalleryImage>();
                        int count = files.Count - 1;
                        int num = 0;
                        string[] allKeys = files.AllKeys;
                        for (i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str4 = allKeys[i];
                            if (num <= count)
                            {
                                if (!str4.Equals("Image"))
                                {
                                    HttpPostedFileBase item = files[num];
                                    if (item.ContentLength > 0)
                                    {
                                        GalleryImageViewModel galleryImageViewModel = new GalleryImageViewModel()
                                        {
                                            PostId = post.Id
                                        };
                                        string str5 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                                        string str6 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}/{1}/", Contains.PostFolder, str), str5, new int?(ImageSize.WithOrignalSize), new int?(ImageSize.HeighthOrignalSize), false);
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}/{1}/", Contains.PostFolder, str), str6, new int?(ImageSize.WithThumbnailSize), new int?(ImageSize.HeightThumbnailSize), false);
                                        galleryImageViewModel.ImageThumbnail = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str6);
                                        galleryImageViewModel.ImagePath = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str5);
                                        galleryImageViewModel.OrderDisplay = num;
                                        galleryImageViewModel.Status = 1;
                                        galleryImageViewModel.Title = post.Title;
                                        galleryImages.Add(Mapper.Map<GalleryImage>(galleryImageViewModel));
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
                    Post post1 = Mapper.Map<PostViewModel, Post>(post);
                    post1.GalleryImages = galleryImages;
                    this._postService.Create(post1);
                    return base.RedirectToAction("PostManagement");
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Post.Create: ", exception.Message));
                base.ModelState.AddModelError("", exception.Message);
            }
            return base.View(post);
        }

        public ActionResult DeleteGallery(int postId, int galleryId)
        {
            ActionResult actionResult;
            if (base.Request.IsAjaxRequest())
            {
                try
                {
                    if (this._postService.Get((Post x) => x.Id == postId && x.CreatedBy.Equals(this.HttpContext.User.Identity.Name), false) == null)
                    {
                        return base.Json(new { success = false });
                    }
                    else
                    {
                        GalleryImage galleryImage = this._galleryService.Get((GalleryImage x) => x.PostId == postId && x.Id == galleryId, false);
                        this._galleryService.Delete(galleryImage);
                        string str = base.Server.MapPath(string.Concat("~/", galleryImage.ImagePath));
                        string str1 = base.Server.MapPath(string.Concat("~/", galleryImage.ImageThumbnail));
                        System.IO.File.Delete(str);
                        System.IO.File.Delete(str1);
                        actionResult = base.Json(new { success = true });
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    actionResult = base.Json(new { success = false, messages = exception.Message });
                }
                return actionResult;
            }
            return base.Json(new { success = false });
        }

        [HttpGet]
        public ActionResult EditPost(int Id)
        {
            PostViewModel postViewModel = Mapper.Map<Post, PostViewModel>(this._postService.Get((Post x) => x.Id == Id && x.CreatedBy.Equals(this.HttpContext.User.Identity.Name), false));
            return base.View(postViewModel);
        }

        public ActionResult EditPost(PostViewModel postView)
        {
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                }
                else
                {
                    Post byId = this._postService.GetById(postView.Id);
                    string str = postView.Title.NonAccent();
                    IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
                    postView.SeoUrl = postView.Title.NonAccent();
                    if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != postView.Id))
                    {
                        PostViewModel postViewModel = postView;
                        postViewModel.SeoUrl = string.Concat(postViewModel.SeoUrl, "-", bySeoUrl.Count<MenuLink>());
                    }
                    HttpFileCollectionBase files = base.Request.Files;
                    if (postView.Image != null && postView.Image.ContentLength > 0)
                    {
                        string str1 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                        string str2 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                        string str3 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                        this._imagePlugin.CropAndResizeImage(postView.Image, string.Format("{0}/{1}/", Contains.PostFolder, str), str1, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
                        this._imagePlugin.CropAndResizeImage(postView.Image, string.Format("{0}/{1}/", Contains.PostFolder, str), str2, new int?(ImageSize.WithMediumSize), new int?(ImageSize.HeightMediumSize), false);
                        this._imagePlugin.CropAndResizeImage(postView.Image, string.Format("{0}/{1}/", Contains.PostFolder, str), str3, new int?(ImageSize.WithSmallSize), new int?(ImageSize.HeightSmallSize), false);
                        postView.ImageBigSize = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str1);
                        postView.ImageMediumSize = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str2);
                        postView.ImageSmallSize = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str3);
                    }
                    int? menuId = postView.MenuId;
                    int i = 0;
                    if ((menuId.GetValueOrDefault() > i ? menuId.HasValue : false))
                    {
                        IMenuLinkService menuLinkService = this._menuLinkService;
                        menuId = postView.MenuId;
                        MenuLink menuLink = menuLinkService.GetById(menuId.Value);
                        postView.VirtualCatUrl = menuLink.VirtualSeoUrl;
                        postView.VirtualCategoryId = menuLink.VirtualId;
                    }
                    List<GalleryImage> galleryImages = new List<GalleryImage>();
                    if (files.Count > 0)
                    {
                        int count = files.Count - 1;
                        int num = 0;
                        string[] allKeys = files.AllKeys;
                        for (i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str4 = allKeys[i];
                            if (num <= count)
                            {
                                if (!str4.Equals("Image"))
                                {
                                    HttpPostedFileBase item = files[num];
                                    if (item.ContentLength > 0)
                                    {
                                        GalleryImageViewModel galleryImageViewModel = new GalleryImageViewModel()
                                        {
                                            PostId = postView.Id
                                        };
                                        string str5 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                                        string str6 = string.Format("{0}-{1}", str, App.Utils.Utils.GetTime());
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}/{1}/", Contains.PostFolder, str), str5, new int?(ImageSize.WithOrignalSize), new int?(ImageSize.HeighthOrignalSize), false);
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}/{1}/", Contains.PostFolder, str), str6, new int?(ImageSize.WithThumbnailSize), new int?(ImageSize.HeightThumbnailSize), false);
                                        galleryImageViewModel.ImageThumbnail = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str6);
                                        galleryImageViewModel.ImagePath = string.Format("{0}/{1}/{2}", Contains.PostFolder, str, str5);
                                        galleryImageViewModel.OrderDisplay = num;
                                        galleryImageViewModel.Status = 1;
                                        galleryImageViewModel.Title = postView.Title;
                                        galleryImages.Add(Mapper.Map<GalleryImage>(galleryImageViewModel));
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
                    if (galleryImages.IsAny<GalleryImage>())
                    {
                        byId.GalleryImages = galleryImages;
                    }
                    byId = Mapper.Map<PostViewModel, Post>(postView, byId);
                    this._postService.Update(byId);
                    ((dynamic)base.ViewBag).Message = "Cập nhật tin rao thành công";
                    return base.View(postView);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                base.ModelState.AddModelError("", exception.Message);
                ExtentionUtils.Log(string.Concat("Post.Edit: ", exception.Message));
            }
            ((dynamic)base.ViewBag).Message = "Cập nhật tin rao KHÔNG thành công";
            return base.View(postView);
        }

        public JsonResult GetDistrictByProvinceId(int provinceId)
        {
            var byProvinceId =
                from x in this._districtService.GetByProvinceId(provinceId)
                select new { Id = x.Id, Name = x.Name };
            return base.Json(byProvinceId);
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            base.AuthenticationManager.SignOut(new string[0]);
            return base.RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("CreatePost"))
            {
                IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType != 5 && x.TemplateType != 1, true);
                ((dynamic)base.ViewBag).MenuList = menuLinks;
            }
            if (filterContext.RouteData.Values["action"].Equals("EditPost"))
            {
                IEnumerable<MenuLink> menuLinks1 = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType != 5 && x.TemplateType != 1, true);
                ((dynamic)base.ViewBag).MenuList = menuLinks1;
            }
        }

        [HttpGet]
        public ActionResult PostManagement(int page = 1)
        {
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "CreatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = 10,
                TotalRecord = 0
            };
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            expression = expression.And<Post>((Post x) => x.CreatedBy.Equals(this.HttpContext.User.Identity.Name));
            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);
            if (posts.IsAny<Post>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("PostManagement", "Account", new { page = i }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
                ((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
            }
            return base.View(posts);
        }

        [HttpPost]
        public ActionResult SearchPost(string keywords, int? status = null, DateTime? from = null, DateTime? to = null)
        {
            ((dynamic)base.ViewBag).Keywords = keywords;
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "CreatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 10,
                TotalRecord = 0
            };
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            expression = expression.And<Post>((Post x) => x.CreatedBy.Equals(this.HttpContext.User.Identity.Name));
            if (status.HasValue)
            {
                expression = expression.And<Post>((Post x) => (int?)x.Status == status);
            }
            if (from.HasValue)
            {
                expression = expression.And<Post>((Post x) => x.StartDate >= from);
            }
            if (to.HasValue)
            {
                expression = expression.And<Post>((Post x) => x.EndDate <= to);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                expression = expression.And<Post>((Post x) => x.Title.Contains(keywords));
            }
            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);
            if (posts.IsAny<Post>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, 1, paging.TotalRecord, (int i) => base.Url.Action("PostManagement", "Account", new { page = i }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
                ((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
            }
            return base.View("PostManagement", posts);
        }
    }
}