using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoFacMvc.Common.Models;

namespace AutoFacMvc.Attributes
{
    public class UserPermissionAttribute : AuthorizeAttribute
    {
        protected string ControllerName = string.Empty;

        protected string ActionName = string.Empty;

        protected bool IsAjax = false;

        protected bool IsDebug = ConfigurationManager.AppSettings["IsDebug"] != null && bool.Parse(ConfigurationManager.AppSettings["IsDebug"]);

        #region Session相关

        protected bool IsUserLogin()
        {
            return GetUserSession() != null;
        }

        protected SessionInfo GetUserSession()
        {
            return HttpContext.Current.Session[SystemKeys.UserSession] as SessionInfo;
        }

        #endregion

        /// <summary>
        /// 授权对象
        /// </summary>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IsAjax = filterContext.HttpContext.Request.IsAjaxRequest();
            ControllerName = filterContext.RouteData.Values["controller"]?.ToString();
            ActionName = filterContext.RouteData.Values["action"]?.ToString();

            // 调试模式
            if (IsDebug)
            {
                HttpContext.Current.Session[SystemKeys.UserSession] = new SessionInfo { Id = 1, UserName = "admin", RealName = "Admin" };
                return;
            }
                

            // 放过 不需要登陆的页面 (找回密码页面等等...)
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0)
            {
                filterContext.HttpContext.SkipAuthorization = true;
                return;
            }

            // 拦截 未登录用户
            if (!IsUserLogin())
            {
                filterContext.Result = new ContentResult() { Content = SystemKeys.UserLogin };
                return;
            }

            // 拦截 其他权限 ...

            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// 授权逻辑
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
        }

        /// <summary>
        /// 无权操作
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}