using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.SeoSitemap;
using App.SeoSitemap.Enum;
using App.SeoSitemap.Images;
using App.Service.Menu;
using App.Service.News;
using App.Service.Post;
using App.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class SiteMapController : Controller
	{
		private readonly IMenuLinkService _menuLinkService;

		private readonly IPostService _postService;

		private readonly INewsService _newsService;

		private readonly ISitemapProvider _sitemapProvider;

		public SiteMapController(IMenuLinkService menuLinkService, IPostService postService, INewsService newsService, ISitemapProvider sitemapProvider)
		{
			this._menuLinkService = menuLinkService;
			this._postService = postService;
			this._newsService = newsService;
			this._sitemapProvider = sitemapProvider;
		}

		public ActionResult Index()
		{
			List<SitemapNode> sitemapNodes = new List<SitemapNode>();
			IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1, true);
			if (menuLinks.IsAny<MenuLink>())
			{
				foreach (MenuLink menuLink in menuLinks)
				{
					sitemapNodes.Add(new SitemapNode("Normal")
					{
						Url = base.Url.Action("GetContent", "Menu", new { menu = menuLink.SeoUrl }, base.Request.Url.Scheme),
						ChangeFrequency = new ChangeFrequency?(ChangeFrequency.Daily),
						Priority = new decimal?(new decimal(8, 0, 0, false, 1)),
						LastModificationDate = (menuLink.UpdatedDate.HasValue ? menuLink.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
					});
				}
			}
			return this._sitemapProvider.CreateSitemap(new SitemapModel(sitemapNodes));
		}

		public ActionResult SiteMapImage()
		{
			List<SitemapImage> sitemapImages = new List<SitemapImage>();
			string item = ConfigurationManager.AppSettings["SiteName"];
			IOrderedEnumerable<Post> posts = 
				from x in this._postService.FindBy((Post x) => x.Status == 1, true)
				orderby x.CreatedDate descending
				select x;
			if (posts.IsAny<Post>())
			{
				foreach (Post post in posts)
				{
					sitemapImages.Add(new SitemapImage(string.Concat(item, post.ImageMediumSize))
					{
						Caption = post.Title,
						Title = post.Title
					});
					if (!post.GalleryImages.IsAny<GalleryImage>())
					{
						continue;
					}
					foreach (GalleryImage galleryImage in post.GalleryImages)
					{
						sitemapImages.Add(new SitemapImage(string.Concat(item, galleryImage.ImagePath))
						{
							Caption = post.Title,
							Title = post.Title
						});
					}
				}
			}
			IOrderedEnumerable<News> news = 
				from x in this._newsService.FindBy((News x) => x.Status == 1, true)
				orderby x.CreatedDate descending
				select x;
			if (news.IsAny<News>())
			{
				foreach (News news1 in news)
				{
					if (string.IsNullOrEmpty(news1.ImageSmallSize))
					{
						continue;
					}
					sitemapImages.Add(new SitemapImage(string.Concat(item, news1.ImageSmallSize))
					{
						Caption = news1.Title,
						Title = news1.Title
					});
				}
			}
			return this._sitemapProvider.CreateSitemap(new SitemapModel(new List<SitemapNode>()
			{
				new SitemapNode(string.Empty)
				{
					Images = sitemapImages
				}
			}));
		}

		public ActionResult SiteMapXml()
		{
			DateTime value;
			string str;
			string empty;
			string str1;
			List<SitemapNode> sitemapNodes = new List<SitemapNode>();
			string item = ConfigurationManager.AppSettings["SiteName"];
			sitemapNodes.Add(new SitemapNode(string.Empty)
			{
				Url = item,
				ChangeFrequency = new ChangeFrequency?(ChangeFrequency.Always),
				Priority = new decimal?(1)
			});
			IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1, true);
			if (menuLinks.IsAny<MenuLink>())
			{
				foreach (MenuLink menuLink in menuLinks)
				{
					SitemapNode sitemapNode = new SitemapNode(string.Empty)
					{
						Url = base.Url.Action("GetContent", "Menu", new { menu = menuLink.SeoUrl }, base.Request.Url.Scheme),
						ChangeFrequency = new ChangeFrequency?(ChangeFrequency.Daily),
						Priority = new decimal?(new decimal(8, 0, 0, false, 1))
					};
					if (menuLink.UpdatedDate.HasValue)
					{
						value = menuLink.UpdatedDate.Value;
						str1 = value.ToString("yyyy-MM-dd");
					}
					else
					{
						str1 = string.Empty;
					}
					sitemapNode.LastModificationDate = str1;
					sitemapNodes.Add(sitemapNode);
				}
			}
			IOrderedEnumerable<Post> posts = 
				from x in this._postService.FindBy((Post x) => x.Status == 1, true)
				orderby x.CreatedDate descending
				select x;
			if (posts.IsAny<Post>())
			{
				foreach (Post post in posts)
				{
					SitemapNode sitemapNode1 = new SitemapNode(string.Empty)
					{
						Url = base.Url.Action("PostDetail", "Post", new { seoUrl = post.SeoUrl }, base.Request.Url.Scheme),
						ChangeFrequency = new ChangeFrequency?(ChangeFrequency.Daily),
						Priority = new decimal?(new decimal(5, 0, 0, false, 1))
					};
					if (post.UpdatedDate.HasValue)
					{
						value = post.UpdatedDate.Value;
						empty = value.ToString("yyyy-MM-dd");
					}
					else
					{
						empty = string.Empty;
					}
					sitemapNode1.LastModificationDate = empty;
					sitemapNodes.Add(sitemapNode1);
				}
			}
			IOrderedEnumerable<News> news = 
				from x in this._newsService.FindBy((News x) => x.Status == 1, true)
				orderby x.CreatedDate descending
				select x;
			if (news.IsAny<News>())
			{
				foreach (News news1 in news)
				{
					SitemapNode sitemapNode2 = new SitemapNode(string.Empty)
					{
						Url = base.Url.Action("NewsDetail", "News", new { seoUrl = news1.SeoUrl }, base.Request.Url.Scheme),
						ChangeFrequency = new ChangeFrequency?(ChangeFrequency.Daily),
						Priority = new decimal?(new decimal(5, 0, 0, false, 1))
					};
					if (news1.UpdatedDate.HasValue)
					{
						value = news1.UpdatedDate.Value;
						str = value.ToString("yyyy-MM-dd");
					}
					else
					{
						str = string.Empty;
					}
					sitemapNode2.LastModificationDate = str;
					sitemapNodes.Add(sitemapNode2);
				}
			}
			return this._sitemapProvider.CreateSitemap(new SitemapModel(sitemapNodes));
		}
	}
}