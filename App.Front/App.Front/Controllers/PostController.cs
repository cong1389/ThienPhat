using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Framework.Ultis;
using App.Front.Models;
using App.Service.Common;
using App.Service.Gallery;
using App.Service.Language;
using App.Service.Menu;
using App.Service.Post;
using App.Utils;
using App.Utils.MVCHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class PostController : FrontBaseController
    {
        private readonly IPostService _postService;

        private readonly IMenuLinkService _menuLinkService;

        private readonly IGalleryService _galleryService;

        private readonly IWorkContext _workContext;

        public PostController(
            IPostService postService
            , IMenuLinkService menuLinkService
            , IGalleryService galleryService
             , IWorkContext workContext)
        {
            this._postService = postService;
            this._menuLinkService = menuLinkService;
            this._galleryService = galleryService;
            this._workContext = workContext;
        }

        public ActionResult FillterProduct(string attribute = null)
        {
            return base.PartialView();
        }

        [PartialCache("Medium")]
        public ActionResult GetAccesssoriesHome(int page, string id)
        {
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            expression = expression.And<Post>((Post x) => x.Status == 1);
            expression = expression.And<Post>((Post x) => x.VirtualCategoryId.Contains(id));
            expression = expression.And<Post>((Post x) => x.ProductHot);
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "UpdatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = 4,
                TotalRecord = 0
            };
            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);
            if (!posts.IsAny<Post>())
            {
                return base.Json(new { success = true, data = "" }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_SlideProductHome", posts), success = true }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetCareerHomePage(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<int>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId), (Post x) => x.ViewCount);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return this.PartialView("GetPostRelative", posts);
        }

        public ActionResult GetGallery(int postId, int typeId)
        {
            IEnumerable<GalleryImage> galleryImages = this._galleryService.FindBy((GalleryImage x) => x.AttributeValueId == typeId && x.PostId == postId, false);
            if (!galleryImages.IsAny<GalleryImage>())
            {
                return base.Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialGallery", galleryImages), success = true }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetNewProductRelative(double? price, int productId)
        {
            double? nullable;
            double? nullable1;
            double? nullable2;
            double? nullable3 = price;
            double num = (double)2000000;
            if (nullable3.HasValue)
            {
                nullable1 = new double?(nullable3.GetValueOrDefault() - num);
            }
            else
            {
                nullable = null;
                nullable1 = nullable;
            }
            double? nullable4 = nullable1;
            nullable3 = price;
            num = (double)2000000;
            if (nullable3.HasValue)
            {
                nullable2 = new double?(nullable3.GetValueOrDefault() + num);
            }
            else
            {
                nullable = null;
                nullable2 = nullable;
            }
            double? nullable5 = nullable2;
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.Price >= nullable4 && x.Price <= nullable5 && x.Id != productId, (Post x) => x.UpdatedDate);

            if (top == null)
                return HttpNotFound();

            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.PartialView(posts);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetNewProductRelative2(string virtualId, int productId)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            List<Post> lstPost = new List<Post>();
            IEnumerable<Post> iePost = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId) && x.Id != productId, (Post x) => x.UpdatedDate);

            if (iePost == null)
                return HttpNotFound();

            IEnumerable<Post> postLocalized = null;
            if (iePost.IsAny<Post>())
            {
                postLocalized = from x in iePost
                                select new Post()
                                {
                                    Id = x.Id,
                                    MenuId = x.MenuId,
                                    VirtualCategoryId = x.VirtualCategoryId,
                                    Language = x.Language,
                                    Status = x.Status,
                                    SeoUrl = x.SeoUrl,
                                    ImageBigSize = x.ImageBigSize,
                                    ImageMediumSize = x.ImageMediumSize,
                                    ImageSmallSize = x.ImageSmallSize,
                                    Price = x.Price,
                                    Discount = x.Discount,
                                    ProductHot = x.ProductHot,
                                    OutOfStock = x.OutOfStock,
                                    ProductNew = x.ProductNew,
                                    VirtualCatUrl = x.VirtualCatUrl,
                                    StartDate = x.StartDate,
                                    PostType = x.PostType,
                                    OldOrNew = x.OldOrNew,
                                    MenuLink = x.MenuLink,

                                    Title = x.GetLocalizedByLocaleKey(x.Title, x.Id, languageId, "Post", "Title"),
                                    ProductCode = x.GetLocalizedByLocaleKey(x.ProductCode, x.Id, languageId, "Post", "ProductCode"),
                                    TechInfo = x.GetLocalizedByLocaleKey(x.TechInfo, x.Id, languageId, "Post", "TechInfo"),
                                    ShortDesc = x.GetLocalizedByLocaleKey(x.ShortDesc, x.Id, languageId, "Post", "ShortDesc"),
                                    Description = x.GetLocalizedByLocaleKey(x.Description, x.Id, languageId, "Post", "Description"),
                                    MetaTitle = x.GetLocalizedByLocaleKey(x.MetaTitle, x.Id, languageId, "Post", "MetaTitle"),
                                    MetaKeywords = x.GetLocalizedByLocaleKey(x.MetaKeywords, x.Id, languageId, "Post", "MetaKeywords"),
                                    MetaDescription = x.GetLocalizedByLocaleKey(x.MetaDescription, x.Id, languageId, "Post", "MetaDescription")
                                };

                lstPost.AddRange(postLocalized);
            }
            return base.PartialView(lstPost);
        }

        [ChildActionOnly]
        public ActionResult GetPostAccessory(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId), (Post x) => x.UpdatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return this.PartialView("_SlideProductHome", posts);
        }

        public ActionResult GetPostByCategory(string virtualCategoryId, int page, string title, string attrs, string prices, string proattrs, string keywords)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            expression = expression.And<Post>((Post x) => x.Status == 1);
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "CreatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            if (page == 1)
            {
                dynamic viewBag = base.ViewBag;
                IPostService postService = this._postService;
                Expression<Func<Post, bool>> productNew = (Post x) => x.ProductNew && x.Status == 1;
                viewBag.HotCard = postService.GetTop<DateTime>(3, productNew, (Post x) => x.CreatedDate).ToList<Post>();
            }
            if (!string.IsNullOrEmpty(attrs))
            {
                string[] strArrays = attrs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> list = (
                    from s in strArrays
                    select int.Parse(s)).ToList<int>();
                expression = expression.And<Post>((Post x) => x.AttributeValues.Count<AttributeValue>((AttributeValue a) => list.Contains(a.Id)) > 0);
                ((dynamic)base.ViewBag).Attributes = list;
            }
            if (!string.IsNullOrEmpty(prices))
            {
                List<double> nums = (
                    from s in prices.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    select double.Parse(s)).ToList<double>();
                double item = nums[0];
                double num = nums[1];
                expression = expression.And<Post>((Post x) => x.Price >= (double?)item && x.Price <= (double?)num);
                ((dynamic)base.ViewBag).Prices = nums;
            }
            if (!string.IsNullOrEmpty(proattrs))
            {
                string[] strArrays1 = proattrs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> list1 = (
                    from s in strArrays1
                    select int.Parse(s)).ToList<int>();
                expression = expression.And<Post>((Post x) => list1.Contains(x.Id));
                ((dynamic)base.ViewBag).ProAttributes = list1;
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                expression = expression.And<Post>((Post x) => x.Title.Contains(keywords));
            }
            expression = expression.And<Post>((Post x) => x.VirtualCategoryId.Contains(virtualCategoryId));
            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);

            if (posts == null)
                return HttpNotFound();

            string[] strArrays2 = virtualCategoryId.Split(new char[] { '/' });
            for (int i1 = 0; i1 < (int)strArrays2.Length; i1++)
            {
                string str = strArrays2[i1];
                MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str) && !x.MenuName.Equals(title), false);
                if (menuLink != null)
                {
                    breadCrumbs.Add(new BreadCrumb()
                    {
                        Title = menuLink.GetLocalizedByLocaleKey(menuLink.MenuName, menuLink.Id, languageId, "MenuLink", "MenuName"),
                        Current = false,
                        Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                    });
                }
            }
            breadCrumbs.Add(new BreadCrumb()
            {
                Current = true,
                Title = title
            });
            ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;

            IEnumerable<Post> postLocalized = null;
            if (posts.IsAny<Post>())
            {
                postLocalized = from x in posts
                         select new Post()
                         {
                             Id = x.Id,
                             MenuId = x.MenuId,
                             VirtualCategoryId = x.VirtualCategoryId,
                             Language = x.Language,
                             Status = x.Status,
                             SeoUrl = x.SeoUrl,
                             ImageBigSize = x.ImageBigSize,
                             ImageMediumSize = x.ImageMediumSize,
                             ImageSmallSize = x.ImageSmallSize,
                             Price = x.Price,
                             Discount = x.Discount,
                             ProductHot = x.ProductHot,
                             OutOfStock = x.OutOfStock,
                             ProductNew = x.ProductNew,
                             VirtualCatUrl = x.VirtualCatUrl,
                             StartDate = x.StartDate,
                             PostType = x.PostType,
                             OldOrNew = x.OldOrNew,
                             MenuLink = x.MenuLink,

                             Title = x.GetLocalizedByLocaleKey(x.Title, x.Id, languageId, "Post", "Title"),
                             ProductCode = x.GetLocalizedByLocaleKey(x.ProductCode, x.Id, languageId, "Post", "ProductCode"),
                             TechInfo = x.GetLocalizedByLocaleKey(x.TechInfo, x.Id, languageId, "Post", "TechInfo"),
                             ShortDesc = x.GetLocalizedByLocaleKey(x.ShortDesc, x.Id, languageId, "Post", "ShortDesc"),
                             Description = x.GetLocalizedByLocaleKey(x.Description, x.Id, languageId, "Post", "Description"),
                             MetaTitle = x.GetLocalizedByLocaleKey(x.MetaTitle, x.Id, languageId, "Post", "MetaTitle"),
                             MetaKeywords = x.GetLocalizedByLocaleKey(x.MetaKeywords, x.Id, languageId, "Post", "MetaKeywords"),
                             MetaDescription = x.GetLocalizedByLocaleKey(x.MetaDescription, x.Id, languageId, "Post", "MetaDescription")
                         };

                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
                ((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
                ((dynamic)base.ViewBag).MenuId = postLocalized.ElementAt(0).MenuId;
            }
            ((dynamic)base.ViewBag).Title = title;
            return base.PartialView(postLocalized);
        }

        [OutputCache(CacheProfile = "Medium")]
        public ActionResult GetPostForYou()
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime>(20, (Post x) => x.Status == 1 && x.PostType == 1, (Post x) => x.CreatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialPostItems", posts), success = true }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "Medium")]
        public ActionResult GetPostLatest()
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime>(20, (Post x) => x.Status == 1, (Post x) => x.CreatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialPostItems", posts), success = true }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetPostRelative(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId), (Post x) => x.UpdatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.PartialView(posts);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetPostSameMenu(int menuId, int postId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime>(6, (Post x) => x.Status == 1 && x.MenuId == menuId && x.Id != postId, (Post x) => x.CreatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.PartialView(posts);
        }

        public ActionResult GetPriceProduct(int proId, int at)
        {
            GalleryImage galleryImage = this._galleryService.Get((GalleryImage x) => x.PostId == proId && x.AttributeValueId == at, false);
            if (galleryImage == null)
            {
                return base.Json("Liên hệ");
            }
            if (!galleryImage.Price.HasValue)
            {
                return base.Json("Liên hệ");
            }
            return base.Json(string.Format("{0:##,###0 VND}", galleryImage.Price));
        }

        public ActionResult GetProductHot()
        {
            IEnumerable<Post> top = this._postService.GetTop(6, (Post x) => x.Status == 1 && x.ProductHot);
            return base.PartialView(top);
        }

        public ActionResult GetProductNew()
        {
            IEnumerable<Post> top = this._postService.GetTop(6, (Post x) => x.Status == 1 && x.ProductNew);
            return base.PartialView(top);
        }

        [PartialCache("Medium")]
        public ActionResult GetProductNewHome(int page, string id)
        {
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            expression = expression.And<Post>((Post x) => x.Status == 1);
            expression = expression.And<Post>((Post x) => x.VirtualCategoryId.Contains(id));
            expression = expression.And<Post>((Post x) => x.ProductHot);
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "UpdatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = 4,
                TotalRecord = 0
            };
            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);
            if (!posts.IsAny<Post>())
            {
                return base.Json(new { success = true, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new
            {
                data = this.RenderRazorViewToString("_SlideProductHome",
                from x in posts
                orderby x.OrderDisplay descending
                select x),
                success = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Get product trang chủ
        public ActionResult GetProductHome(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId) && x.ProductHot, (Post x) => x.UpdatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            top = from x in posts orderby x.OrderDisplay descending select x;

            return base.Json(new { data = this.RenderRazorViewToString("_PartialFixItemContent", top), success = true }, JsonRequestBehavior.AllowGet);
            //return this.PartialView("_PartialFixItemContent",
            //    from x in posts
            //    orderby x.OrderDisplay descending
            //    select x);
        }

        [ChildActionOnly]
        public ActionResult GetProductNewTabHome(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId) && x.ProductHot, (Post x) => x.UpdatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return this.PartialView("_SlideProductHome",
                from x in posts
                orderby x.OrderDisplay descending
                select x);
        }

        [OutputCache(CacheProfile = "Medium")]
        public ActionResult PostDetail(string seoUrl)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            Post post = this._postService.Get((Post x) => x.SeoUrl.Equals(seoUrl), false);

            if (post == null)
                return HttpNotFound();

            Post postLocalized = null;

            if (post != null)
            {
                postLocalized = new Post
                {
                    Id = post.Id,
                    MenuId = post.MenuId,
                    VirtualCategoryId = post.VirtualCategoryId,
                    Language = post.Language,
                    Status = post.Status,
                    SeoUrl = post.SeoUrl,
                    ImageBigSize = post.ImageBigSize,
                    ImageMediumSize = post.ImageMediumSize,
                    ImageSmallSize = post.ImageSmallSize,
                    Price = post.Price,
                    Discount = post.Discount,
                    ProductHot = post.ProductHot,
                    OutOfStock = post.OutOfStock,
                    ProductNew = post.ProductNew,
                    VirtualCatUrl = post.VirtualCatUrl,
                    StartDate = post.StartDate,
                    PostType = post.PostType,
                    OldOrNew = post.OldOrNew,
                    MenuLink = post.MenuLink,

                    Title = post.GetLocalizedByLocaleKey(post.Title, post.Id, languageId, "Post", "Title"),
                    ProductCode = post.GetLocalizedByLocaleKey(post.ProductCode, post.Id, languageId, "Post", "ProductCode"),
                    TechInfo = post.GetLocalizedByLocaleKey(post.TechInfo, post.Id, languageId, "Post", "TechInfo"),
                    ShortDesc = post.GetLocalizedByLocaleKey(post.ShortDesc, post.Id, languageId, "Post", "ShortDesc"),
                    Description = post.GetLocalizedByLocaleKey(post.Description, post.Id, languageId, "Post", "Description"),
                    MetaTitle = post.GetLocalizedByLocaleKey(post.MetaTitle, post.Id, languageId, "Post", "MetaTitle"),
                    MetaKeywords = post.GetLocalizedByLocaleKey(post.MetaKeywords, post.Id, languageId, "Post", "MetaKeywords"),
                    MetaDescription = post.GetLocalizedByLocaleKey(post.MetaDescription, post.Id, languageId, "Post", "MetaDescription")
                };

                Post viewCount = post;
                viewCount.ViewCount = viewCount.ViewCount + 1;
                this._postService.Update(post);
                string[] strArrays = post.VirtualCategoryId.Split(new char[] { '/' });
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str), false);
                    breadCrumbs.Add(new BreadCrumb()
                    {
                        Title = menuLink.GetLocalizedByLocaleKey(menuLink.MenuName, menuLink.Id, languageId, "MenuLink", "MenuName"),
                        Current = false,
                        Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                    });
                }
                breadCrumbs.Add(new BreadCrumb()
                {
                    Current = true,
                    Title = postLocalized.Title
                });
                ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
                ((dynamic)base.ViewBag).Title = postLocalized.Title;
                ((dynamic)base.ViewBag).KeyWords = postLocalized.MetaKeywords;
                ((dynamic)base.ViewBag).SiteUrl = base.Url.Action("PostDetail", "Post", new { seoUrl = seoUrl, area = "" });
                ((dynamic)base.ViewBag).Description = postLocalized.MetaTitle;
                ((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", postLocalized.ImageMediumSize));
                ((dynamic)base.ViewBag).MenuId = postLocalized.MenuId;
            }
            return base.View(postLocalized);
        }

        [OutputCache(CacheProfile = "Medium")]
        public ActionResult SearchResult(string catUrl, string parameters, int page)
        {
            HttpCookie httpCookie = base.HttpContext.Request.Cookies.Get("system_search");
            if (!base.Request.Cookies.ExistsCokiee("system_search"))
            {
                return base.View();
            }
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "CreatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            SeachConditions seachCondition = JsonConvert.DeserializeObject<SeachConditions>(base.Server.UrlDecode(httpCookie.Value));
            expression = expression.And<Post>((Post x) => (int?)x.MenuId == seachCondition.CategoryId);
            MenuLink byId = this._menuLinkService.GetById(seachCondition.CategoryId.Value);
            ((dynamic)base.ViewBag).KeyWords = byId.MetaKeywords;
            ((dynamic)base.ViewBag).SiteUrl = base.Url.Action("GetContent", "Menu", new { catUrl = catUrl, parameters = parameters, page = page, area = "" });
            ((dynamic)base.ViewBag).Description = byId.MetaDescription;
            ((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", byId.ImageUrl));
            ((dynamic)base.ViewBag).VirtualId = byId.CurrentVirtualId;
            string menuName = byId.MenuName;
            if (!string.IsNullOrEmpty(seachCondition.Keywords))
            {
                expression = expression.And<Post>((Post x) => x.Title.Contains(seachCondition.Keywords) || x.Description.Contains(seachCondition.Keywords));
            }
            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);
            ((dynamic)base.ViewBag).PageNumber = page;
            ((dynamic)base.ViewBag).Title = menuName;
            if (posts.IsAny<Post>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
                ((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
            }
            return base.View("GetPostByCategory", posts);
        }
    }
}