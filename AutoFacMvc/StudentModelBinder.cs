using System.Web.Mvc;

namespace AutoFacMvc
{
    public class StudentModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelName == "sessionStudent")
            {
                if (controllerContext.HttpContext.Session["student"] != null)
                {
                    return controllerContext.HttpContext.Session["student"];
                }
            }
            return null;
        }
    }
}



























