using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoFacMvc.Models.ViewModels;

namespace AutoFacMvc.Common.Extensions
{
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString CreatePageItem(this UrlHelper urlHelper,
            PageViewModel pageModel,
            int index,
            bool isCurrentIndex = false,
            bool isDisable = true,
            string content = "")
        {
            var url = urlHelper.Action(pageModel.ActionName, new { currentIndex = index, pageSize = pageModel.PageSize });
            var activeClass = !isCurrentIndex ? string.Empty : "class='active'";
            var disableClass = isDisable ? string.Empty : "class='disabled'";
            url = isDisable ? "href='" + url + "'" : string.Empty;
            var contentString = string.IsNullOrEmpty(content) ? index.ToString() : content;
            return new MvcHtmlString("<li " + activeClass + disableClass + "><a " + url + ">" + contentString + "</a></li>");
        }
    }
}