using App.Admin.Helpers;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.FakeEntity.Gallery;
using App.FakeEntity.Post;
using App.Framework.Ultis;
using App.ImagePlugin;
using App.Service.Attribute;
using App.Service.Gallery;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using App.Service.Post;
using App.Utils;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Admin.Controllers
{
	public class PostController : BaseAdminUploadController
    {
		private readonly IAttributeValueService _attributeValueService;

		private readonly IMenuLinkService _menuLinkService;

		private readonly IPostService _postService;

		private readonly IAttributeService _attributeService;

		private readonly IGalleryService _galleryService;

		private readonly IImagePlugin _imagePlugin;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public PostController(
            IPostService postService
            , IMenuLinkService menuLinkService
            , IAttributeValueService attributeValueService
            , IGalleryService galleryService
            , IImagePlugin imagePlugin
            , IAttributeService attributeService
            , ILanguageService languageService
            , ILocalizedPropertyService localizedPropertyService)
		{
			this._postService = postService;
			this._menuLinkService = menuLinkService;
			this._attributeValueService = attributeValueService;
			this._galleryService = galleryService;
			this._imagePlugin = imagePlugin;
			this._attributeService = attributeService;
            this._languageService = languageService;
            this._localizedPropertyService = localizedPropertyService;
        }

		[RequiredPermisson(Roles="CreatePost")]
		public ActionResult Create()
		{
            var model = new PostViewModel {
                OrderDisplay = 0,
                Status = 1
            };

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreatePost")]
		[ValidateInput(false)]
		public ActionResult Create(PostViewModel model, string ReturnUrl)
		{
			ActionResult action;
			try
			{
				if (!base.ModelState.IsValid)
				{
                    String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                          .Select(v => v.ErrorMessage + " " + v.Exception));
                    base.ModelState.AddModelError("", messages);
                    return base.View(model);
                }
				else //if (!this._postService.FindBy((Post x) => x.ProductCode.Equals(model.ProductCode), true).IsAny<Post>())
				{
					string str = model.Title.NonAccent();
					IEnumerable<Post> bySeoUrl = this._postService.GetBySeoUrl(str);
					model.SeoUrl = model.Title.NonAccent();
					if (bySeoUrl.Any<Post>((Post x) => x.Id != model.Id))
					{
						PostViewModel postViewModel = model;
						postViewModel.SeoUrl = string.Concat(postViewModel.SeoUrl, "-", bySeoUrl.Count<Post>());
					}
					string str1 = str;
					if (str1.Length > 250)
					{
						str1 = AdminHelper.SplitWords(250, str1);
					}
					string str2 = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
					if (model.Image != null && model.Image.ContentLength > 0)
					{
						string str3 = string.Concat(str1, ".jpg");
						string str4 = string.Format("{0}-{1}.jpg", str1, Guid.NewGuid());
						string str5 = string.Format("{0}-{1}.jpg", str1, Guid.NewGuid());
						this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, str2), str3, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
						this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, str2), str4, new int?(ImageSize.WithMediumSize), new int?(ImageSize.HeightMediumSize), false);
						this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, str2), str5, new int?(ImageSize.WithSmallSize), new int?(ImageSize.HeightSmallSize), false);
						model.ImageBigSize = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str3);
						model.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str4);
						model.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str5);
					}
					int? menuId = model.MenuId;
					int i = 0;
					if ((menuId.GetValueOrDefault() > i ? menuId.HasValue : false))
					{
						IMenuLinkService menuLinkService = this._menuLinkService;
						menuId = model.MenuId;
						MenuLink byId = menuLinkService.GetById(menuId.Value);
						model.VirtualCatUrl = byId.VirtualSeoUrl;
						model.VirtualCategoryId = byId.VirtualId;
					}
					HttpFileCollectionBase files = base.Request.Files;
					List<GalleryImage> galleryImages = new List<GalleryImage>();
					if (files.Count > 0)
					{
						int count = files.Count - 1;
						int num = 0;
						string str6 = str;
						string[] allKeys = files.AllKeys;
						for (i = 0; i < (int)allKeys.Length; i++)
						{
							string str7 = allKeys[i];
							if (num <= count)
							{
								if (!str7.Equals("Image"))
								{
									string str8 = str7.Replace("[]", "");
									HttpPostedFileBase item = files[num];
									if (item.ContentLength > 0)
									{
										string item1 = base.Request[str8];
										GalleryImageViewModel galleryImageViewModel = new GalleryImageViewModel()
										{
											PostId = model.Id,
											AttributeValueId = int.Parse(str8)
										};
										string str9 = string.Format("{0}-{1}.jpg", str6, Guid.NewGuid());
										string str10 = string.Format("{0}-{1}.jpg", str6, Guid.NewGuid());
										this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, str2), str9, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
										this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, str2), str10, new int?(ImageSize.WithThumbnailSize), new int?(ImageSize.HeightThumbnailSize), false);
										galleryImageViewModel.ImageThumbnail = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str10);
										galleryImageViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str9);
										galleryImageViewModel.OrderDisplay = num;
										galleryImageViewModel.Status = 1;
										galleryImageViewModel.Title = model.Title;
										galleryImageViewModel.Price = new double?(double.Parse(item1));
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
					List<AttributeValue> attributeValues = new List<AttributeValue>();
					string item2 = base.Request["Values"];
					if (!string.IsNullOrEmpty(item2))
					{
						foreach (string list in item2.Split(new char[] { ',' }).ToList<string>())
						{
							int num1 = int.Parse(list);
							attributeValues.Add(this._attributeValueService.GetById(num1));
						}
					}
					Post modelMap = Mapper.Map<PostViewModel, Post>(model);
					if (galleryImages.IsAny<GalleryImage>())
					{
						modelMap.GalleryImages = galleryImages;
					}
					if (attributeValues.IsAny<AttributeValue>())
					{
						modelMap.AttributeValues = attributeValues;
					}
					this._postService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ProductCode, localized.ProductCode, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.TechInfo, localized.TechInfo, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.SeoUrl, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Post)));
					if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
					{
						action = base.RedirectToAction("Index");
					}
					else
					{
						action = this.Redirect(ReturnUrl);
					}
				}
				//else
				//{
				//	base.ModelState.AddModelError("", "Mã sản phẩm đã tồn tại.");
				//	action = base.View(model);
				//}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("Post.Create: ", exception.Message));
				base.ModelState.AddModelError("", exception.Message);
				return base.View(model);
			}
			return action;
		}

		[RequiredPermisson(Roles="DeletePost")]
		public ActionResult Delete(string[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					List<Post> posts = new List<Post>();
					List<GalleryImage> galleryImages = new List<GalleryImage>();
					string[] strArrays = ids;
					for (int i = 0; i < (int)strArrays.Length; i++)
					{
						int num = int.Parse(strArrays[i]);
						Post post = this._postService.Get((Post x) => x.Id == num, false);
						galleryImages.AddRange(post.GalleryImages.ToList<GalleryImage>());
						post.AttributeValues.ToList<AttributeValue>().ForEach((AttributeValue att) => post.AttributeValues.Remove(att));
						posts.Add(post);
					}
					this._galleryService.BatchDelete(galleryImages);
					this._postService.BatchDelete(posts);

                    //Delete localize
                    for (int i = 0; i < ids.Length; i++)
                    {
                        IEnumerable<LocalizedProperty> ieLocalizedProperty
                           = _localizedPropertyService.GetLocalizedPropertyByEntityId(int.Parse(ids[i]));
                        this._localizedPropertyService.BatchDelete(ieLocalizedProperty);
                    }
                }
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("Post.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreatePost")]
		public ActionResult DeleteGallery(int postId, int galleryId)
		{
			ActionResult actionResult;
			if (!base.Request.IsAjaxRequest())
			{
				return base.Json(new { success = false });
			}
			try
			{
				GalleryImage galleryImage = this._galleryService.Get((GalleryImage x) => x.PostId == postId && x.Id == galleryId, false);
				this._galleryService.Delete(galleryImage);
				string str = base.Server.MapPath(string.Concat("~/", galleryImage.ImagePath));
				string str1 = base.Server.MapPath(string.Concat("~/", galleryImage.ImageThumbnail));
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

		[RequiredPermisson(Roles="CreatePost")]
		public ActionResult Edit(int Id)
		{
			Post byId = this._postService.GetById(Id);

			PostViewModel modelMap = Mapper.Map<Post, PostViewModel>(byId);
			((dynamic)base.ViewBag).Galleries = byId.GalleryImages;

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;                
                locale.Title = modelMap.GetLocalized(x => x.Title, Id, languageId, false, false);
                locale.ProductCode = modelMap.GetLocalized(x => x.ProductCode, Id, languageId, false, false);
                locale.ShortDesc = modelMap.GetLocalized(x => x.ShortDesc, Id, languageId, false, false);
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);
                locale.TechInfo = modelMap.GetLocalized(x => x.TechInfo, Id, languageId, false, false);
                locale.MetaTitle = modelMap.GetLocalized(x => x.MetaTitle, Id, languageId, false, false);
                locale.MetaKeywords = modelMap.GetLocalized(x => x.MetaKeywords, Id, languageId, false, false);
                locale.MetaDescription = modelMap.GetLocalized(x => x.MetaDescription, Id, languageId, false, false);
                locale.SeoUrl = modelMap.GetLocalized(x => x.SeoUrl, Id, languageId, false, false);
            });

            return base.View(modelMap);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreatePost")]
		[ValidateInput(false)]
		public ActionResult Edit(PostViewModel model, string ReturnUrl)
		{
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                          .Select(v => v.ErrorMessage + " " + v.Exception));
                    base.ModelState.AddModelError("", messages);
                    return base.View(model);
                }
                else if (!this._postService.FindBy((Post x) => x.ProductCode.Equals(model.ProductCode) && x.Id != model.Id, true).IsAny<Post>())
                {
                    Post byId = this._postService.GetById(model.Id);
                    string str = model.Title.NonAccent();
                    IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetBySeoUrl(str);
                    model.SeoUrl = model.Title.NonAccent();
                    if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != model.Id))
                    {
                        PostViewModel postViewModel = model;
                        postViewModel.SeoUrl = string.Concat(postViewModel.SeoUrl, "-", bySeoUrl.Count<MenuLink>());
                    }
                    string str1 = str;
                    if (str1.Length > 250)
                    {
                        str1 = AdminHelper.SplitWords(250, str1);
                    }
                    HttpFileCollectionBase files = base.Request.Files;
                    string str2 = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string str3 = string.Format("{0}.jpg", str1);
                        string str4 = string.Format("{0}-{1}.jpg", str1, Guid.NewGuid());
                        string str5 = string.Format("{0}-{1}.jpg", str1, Guid.NewGuid());
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, str2), str3, new int?(ImageSize.WithBigSize), new int?(ImageSize.HeightBigSize), false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, str2), str4, new int?(ImageSize.WithMediumSize), new int?(ImageSize.HeightMediumSize), false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, str2), str5, new int?(ImageSize.WithSmallSize), new int?(ImageSize.HeightSmallSize), false);
                        model.ImageBigSize = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str3);
                        model.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str4);
                        model.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str5);
                    }
                    int? menuId = model.MenuId;
                    int i = 0;
                    if ((menuId.GetValueOrDefault() > i ? menuId.HasValue : false))
                    {
                        IMenuLinkService menuLinkService = this._menuLinkService;
                        menuId = model.MenuId;
                        MenuLink menuLink = menuLinkService.GetById(menuId.Value);
                        model.VirtualCatUrl = menuLink.VirtualSeoUrl;
                        model.VirtualCategoryId = menuLink.VirtualId;
                    }
                    List<GalleryImage> galleryImages = new List<GalleryImage>();
                    if (files.Count > 0)
                    {
                        int count = files.Count - 1;
                        int num = 0;
                        string str6 = str;
                        string[] allKeys = files.AllKeys;
                        for (i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str7 = allKeys[i];
                            if (num <= count)
                            {
                                if (!str7.Equals("Image"))
                                {
                                    string str8 = str7.Replace("[]", "");
                                    HttpPostedFileBase item = files[num];
                                    if (item.ContentLength > 0)
                                    {
                                        string item1 = base.Request[str8];
                                        GalleryImageViewModel galleryImageViewModel = new GalleryImageViewModel()
                                        {
                                            PostId = model.Id,
                                            AttributeValueId = int.Parse(str8)
                                        };
                                        string str9 = string.Format("{0}-{1}.jpg", str6, Guid.NewGuid());
                                        string str10 = string.Format("{0}-{1}.jpg", str6, Guid.NewGuid());
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, str2), str9, new int?(ImageSize.WithBigSize), new int?(ImageSize.WithBigSize), false);
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, str2), str10, new int?(ImageSize.WithThumbnailSize), new int?(ImageSize.HeightThumbnailSize), false);
                                        galleryImageViewModel.ImageThumbnail = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str10);
                                        galleryImageViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.PostFolder, str2, str9);
                                        galleryImageViewModel.OrderDisplay = num;
                                        galleryImageViewModel.Status = 1;
                                        galleryImageViewModel.Title = model.Title;
                                        galleryImageViewModel.Price = new double?(double.Parse(item1));
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
                    List<AttributeValue> attributeValues = new List<AttributeValue>();
                    List<int> nums = new List<int>();
                    string item2 = base.Request["Values"];
                    if (!string.IsNullOrEmpty(item2))
                    {
                        foreach (string list in item2.Split(new char[] { ',' }).ToList<string>())
                        {
                            int num1 = int.Parse(list);
                            nums.Add(num1);
                            attributeValues.Add(this._attributeValueService.GetById(num1));
                        }
                    }
                    if (nums.IsAny<int>())
                    {
                        (
                            from x in byId.AttributeValues
                            where !nums.Contains(x.Id)
                            select x).ToList<AttributeValue>().ForEach((AttributeValue att) => byId.AttributeValues.Remove(att));
                    }
                    if (galleryImages.IsAny<GalleryImage>())
                    {
                        byId.GalleryImages = galleryImages;
                    }
                    if (attributeValues.IsAny<AttributeValue>())
                    {
                        byId.AttributeValues = attributeValues;
                    }
                    Post modelMap = Mapper.Map<PostViewModel, Post>(model, byId);
                    this._postService.Update(byId);
                    if (attributeValues.IsAny<AttributeValue>())
                    {
                        foreach (AttributeValue attributeValue in attributeValues)
                        {
                            GalleryImage nullable = this._galleryService.Get((GalleryImage x) => x.AttributeValueId == attributeValue.Id && x.PostId == model.Id, false);
                            if (nullable == null)
                            {
                                continue;
                            }
                            HttpRequestBase request = base.Request;
                            i = attributeValue.Id;
                            double num2 = double.Parse(request[i.ToString()]);
                            nullable.Price = new double?(num2);
                            this._galleryService.Update(nullable);
                        }
                    }

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ProductCode, localized.ProductCode, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.TechInfo, localized.TechInfo, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.SeoUrl, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Post)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
                else
                {
                    base.ModelState.AddModelError("", "Mã sản phẩm đã tồn tại.");
                    action = base.View(model);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                base.ModelState.AddModelError("", exception.Message);
                ExtentionUtils.Log(string.Concat("Post.Edit: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

		[RequiredPermisson(Roles="ViewPost")]
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
			IEnumerable<Post> posts = this._postService.PagedList(sortingPagingBuilder, paging);
			if (posts != null && posts.Any<Post>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(posts);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.RouteData.Values["action"].Equals("edit") || filterContext.RouteData.Values["action"].Equals("create"))
			{
				IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && (x.TemplateType == 2 || x.TemplateType == 8) && x.ParentId.HasValue, true);
				((dynamic)base.ViewBag).MenuList = menuLinks;
				IEnumerable<App.Domain.Entities.Attribute.Attribute> attributes = this._attributeService.FindBy((App.Domain.Entities.Attribute.Attribute x) => x.Status == 1, false);
				if (attributes.IsAny<App.Domain.Entities.Attribute.Attribute>())
				{
					((dynamic)base.ViewBag).Attributes = attributes;
				}
			}
		}
	}
}