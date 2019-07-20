using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoFacMvc.Attributes;
using AutoFacMvc.Common.Models;

namespace AutoFacMvc.Controllers
{
    /// <summary>
    /// 用户基类控制器
    /// </summary>
    [UserPermission]
    public class UserBaseController : BaseController
    {
        #region 用户Session相关操作

        protected bool IsUserLogin()
        {
            return GetUserSession() != null;
        }

        protected SessionInfo GetUserSession()
        {
            return System.Web.HttpContext.Current.Session[SystemKeys.UserSession] as SessionInfo;
        }

        protected void SetUserSession(SessionInfo info)
        {
            System.Web.HttpContext.Current.Session[SystemKeys.UserSession] = info;
        }

        protected void SetUserLogOut()
        {
            System.Web.HttpContext.Current.Session.Remove(SystemKeys.UserSession);
            System.Web.HttpContext.Current.Session.Abandon();
        }

        #endregion   
    }
}