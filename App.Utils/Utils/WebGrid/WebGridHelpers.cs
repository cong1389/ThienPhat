﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using App.Domain.Entities.Language;
using App.FakeEntity.Language;

namespace App.Utils.WebGrid
{
    public static class WebGridHelpers
    {
        //public static HtmlString WebGridFilter<T>(this HtmlHelper helper,
        //    IEnumerable<T> users, Func<T, string> property,
        //    string headingText) where T : class
        //{
        //    var model = new WebGridFilterModel
        //    {
        //        Users = users.GroupBy(property).Select(g => g.First()),
        //        Property = property,
        //        HeadingText = headingText
        //    };
        //    return helper.Partial("_webGridFilter", model);
        //}

        public static MvcHtmlString EditableTextBox(this HtmlHelper helper,
        string value, LocaleStringResource locale, string name,string id)
        {
            // Text Display
            var span = new TagBuilder("span") { InnerHtml = value };
            span.AddCssClass("cell-value");

            // Input display.
            var formatName = HtmlHelper.GenerateIdFromName(name);

            var uniqueId = String.Format("{0}_{1}", formatName, id);

            var input = helper.TextBox(uniqueId,
                value, new { @class = "hide input-sm" });

            var result = String.Concat(
                span.ToString(TagRenderMode.Normal),
                input.ToHtmlString()
                );

            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString EditableDateTime(this HtmlHelper helper,
            DateTime value, LocaleStringResource locale, string name,string id)
        {
            // Text Display
            var span = new TagBuilder("span") { InnerHtml = value.ToString("yyyy-MM-dd") };
            span.AddCssClass("cell-value");

            // Input display.
            var formatName = HtmlHelper.GenerateIdFromName(name);

            var uniqueId = String.Format("{0}_{1}", formatName, id);

            var input = helper.TextBox(uniqueId, value.ToString("yyyy-MM-dd"),
                new { @type = "date", @class = "hide input-sm" });

            var result = String.Concat(
                span.ToString(TagRenderMode.Normal),
                input.ToHtmlString()
                );
            return MvcHtmlString.Create(result);
        }

    }
}
