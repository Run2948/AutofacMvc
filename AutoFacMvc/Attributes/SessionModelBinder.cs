using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoFacMvc.Common.Models;

namespace AutoFacMvc.Attributes
{
    public class SessionModelBinder : IModelBinder
    {
        /// <summary>使用指定的控制器上下文和绑定上下文将模型绑定到一个值。</summary>
        /// <returns>绑定值。</returns>
        /// <param name="controllerContext">控制器上下文。</param>
        /// <param name="bindingContext">绑定上下文。</param>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(SessionInfo))
            {
                if (controllerContext.HttpContext.Session[SystemKeys.UserSession] != null)
                {
                    return controllerContext.HttpContext.Session[SystemKeys.UserSession];
                }
            }
            return null;
        }
    }
}