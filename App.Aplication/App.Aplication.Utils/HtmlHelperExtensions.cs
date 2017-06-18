using App.Aplication.PagedSort.SortUtils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Aplication.Utils
{
	public static class HtmlHelperExtensions
	{
		public static MvcHtmlString CheckBoxListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, object>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
		{
			TagBuilder tagBuilder = new TagBuilder("ul");
			if (htmlAttributes != null)
			{
				tagBuilder.MergeAttributes<string, object>(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
			}
			tagBuilder.AddCssClass("check-box-list");
			ModelMetadata modelMetadatum = ModelMetadata.FromLambdaExpression<TModel, object>(expression, htmlHelper.ViewData);
			if (selectList == null)
			{
				return MvcHtmlString.Create(tagBuilder.ToString());
			}
			foreach (SelectListItem selectListItem in selectList)
			{
				TagBuilder str = new TagBuilder("li");
				TagBuilder tagBuilder1 = new TagBuilder("label");
				TagBuilder tagBuilder2 = new TagBuilder("input");
				str.AddCssClass("checkbox");
				tagBuilder2.MergeAttribute("type", "checkbox");
				tagBuilder2.MergeAttribute("name", modelMetadatum.PropertyName);
				tagBuilder2.MergeAttribute("value", selectListItem.Value);
				if (selectListItem.Selected)
				{
					tagBuilder2.MergeAttribute("checked", "checked");
				}
				tagBuilder2.GenerateId(modelMetadatum.PropertyName);
				tagBuilder1.InnerHtml = tagBuilder2.ToString(TagRenderMode.SelfClosing);
				TagBuilder tagBuilder3 = tagBuilder1;
				tagBuilder3.InnerHtml = string.Concat(tagBuilder3.InnerHtml, " ", selectListItem.Text);
				str.InnerHtml = tagBuilder1.ToString();
				TagBuilder tagBuilder4 = tagBuilder;
				tagBuilder4.InnerHtml = string.Concat(tagBuilder4.InnerHtml, str.ToString());
			}
			return MvcHtmlString.Create(tagBuilder.ToString());
		}

		public static MvcHtmlString SortExpressionLink(this HtmlHelper helper, string title, string sortExpression, SortDirection direction, object htmlAttributes = null)
		{
			TagBuilder tagBuilder = new TagBuilder("a");
			if (htmlAttributes != null)
			{
				tagBuilder.MergeAttributes<string, object>(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
			}
			TagBuilder tagBuilder1 = new TagBuilder("i");
			tagBuilder1.MergeAttribute("class", "indicator");
			tagBuilder.AddCssClass("sort-expression-link");
			tagBuilder.MergeAttribute("title", title);
			tagBuilder.MergeAttribute("href", string.Concat("#", sortExpression));
			tagBuilder.MergeAttribute("data-sort-expression", sortExpression);
			tagBuilder.MergeAttribute("data-sort-direction", direction.ToString());
			tagBuilder.InnerHtml = string.Concat(title, tagBuilder1.ToString(TagRenderMode.Normal));
			return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
		}
	}
}