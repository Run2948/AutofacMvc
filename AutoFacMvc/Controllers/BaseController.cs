using AutoFacMvc.Common.Models;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AutoFacMvc.Controllers
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    public class BaseController : Controller
    {
        #region 获取应用程序的Debug模式

        protected bool IsDebug = ConfigurationManager.AppSettings["IsDebug"] != null && bool.Parse(ConfigurationManager.AppSettings["IsDebug"]);

        #endregion

        #region 通用返回JsonResult的封装

        /// <summary>
        /// 只返回响应状态和提示信息
        /// </summary>
        /// <param name="code">状态</param>
        /// <param name="msg">消息</param>
        /// <param name="get">是否允许GET请求</param>
        /// <returns></returns>
        protected static JsonResult Build(int code, string msg, bool get = false)
        {
            var js = new JsonResult
            {
                Data = new ResultInfo(code, msg)
            };
            if (get)
                js.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return js;
        }

        /// <summary>
        /// 返回响应状态、消息和数据对象
        /// </summary>
        /// <param name="code">状态</param>
        /// <param name="msg">信息</param>
        /// <param name="data">数据</param>
        /// <param name="get">是否允许GET请求</param>
        /// <returns></returns>
        protected static JsonResult Build(int code, string msg, object data, bool get = false)
        {
            var js = new JsonResult
            {
                Data = new ResultInfo(code, msg, data)
            };
            if (get)
                js.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return js;
        }

        /// <summary>
        /// 返回响应状态、消息、数据和跳转地址
        /// </summary>
        /// <param name="result">ResultInfo实体</param>
        /// <param name="get">是否允许GET请求</param>
        /// <returns></returns>
        protected static JsonResult Build(ResultInfo result, bool get = false)
        {
            var js = new JsonResult
            {
                Data = new ResultInfo(result.Code, result.Msg, result.Data)
            };
            if (get)
                js.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return js;
        }

        /// <summary>
        /// 返回成功状态、消息和数据对象
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected static JsonResult Ok(string msg, object data)
        {
            return Build(code: 1, msg: msg, data: data, get: true);
        }

        /// <summary>
        /// 返回成功状态和数据对象
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected static JsonResult Ok(object data)
        {
            return Ok("Success", data: data);
        }

        /// <summary>
        /// 返回成功状态和消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        protected static JsonResult Ok(string msg)
        {
            return Build(code: 1, msg: msg, get: true);
        }

        /// <summary>
        /// 返回成功状态
        /// </summary>
        /// <returns></returns>
        protected static JsonResult Ok()
        {
            return Ok("Success");
        }

        /// <summary>
        /// 返回失败状态和消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        protected static JsonResult No(string msg)
        {
            return Build(code: 0, msg: msg, get: true);
        }

        /// <summary>
        /// 返回失败状态
        /// </summary>
        /// <returns></returns>
        protected static JsonResult No()
        {
            return No("Failure ");
        }

        #endregion

        #region 通用图片上传

        /// <summary>
        /// 通用图片上传
        /// </summary>
        /// <param name="fileInput">file表单参数名</param>
        /// <param name="folderName">上传目录的名称，如：~/upload/images/teachers/...，则直接填写 teachers</param>
        /// <returns></returns>
        public (bool, string) ImageUpload(string fileInput, string folderName)
        {
            HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
            HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            HttpContext.Response.Charset = "UTF-8";
            try
            {
                if (string.IsNullOrEmpty(fileInput))
                    fileInput = "file";
                if (string.IsNullOrEmpty(folderName))
                    folderName = "files";
                var file = Request.Files[fileInput];
                if (file == null)
                    return (false, "上传图片为空！请勿非法提交");
                if (string.IsNullOrEmpty(file.ContentType) || (file.ContentType).IndexOf("image/", StringComparison.Ordinal) == -1)
                {
                    return (false, "未识别的图片格式！请勿非法提交");
                }
                var result = ImageUploadPro(file, folderName);
                return result;
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

        }

        /// <summary>
        /// 通用图片上传
        /// </summary>
        /// <param name="file">待上传的文件</param>
        /// <param name="folderName">上传目录的名称，如：~/upload/images/teachers/...，则直接填写 teachers</param>
        /// <returns></returns>
        public (bool, string) ImageUploadPro(HttpPostedFileBase file, string folderName)
        {
            HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
            HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            HttpContext.Response.Charset = "UTF-8";
            if (file == null)
                return (false, "上传图片文件不能为空!");

            string fileName = Path.GetFileName(file.FileName); //文件名
            string fileExt = Path.GetExtension(fileName);  //文件后缀名

            if (fileExt == null)
                return (false, "无法识别的图片格式!!");

            fileExt = fileExt.ToLower();
            if (!".jpg".Equals(fileExt) && !".gif".Equals(fileExt) && !".png".Equals(fileExt) && !".ico".Equals(fileExt) && !"jpeg".Equals(fileExt))
                return (false, "上传文件格式错误!!支持后缀为.jpg、.jpeg、png、.gif、.ico格式的图片上传");

            try
            {
                var dir = $"/upload/images/{folderName}/{DateTime.Now:yyyy}/{DateTime.Now:yyyyMM}/{DateTime.Now:yyyyMMdd}/";
                if (!Directory.Exists(HttpContext.Request.MapPath(dir)))
                    Directory.CreateDirectory(HttpContext.Request.MapPath(dir));
                string imagePath = dir + Guid.NewGuid().ToString("N") + fileExt;  //全球唯一标示符，作为文件名
                file.SaveAs(HttpContext.Request.MapPath(imagePath));
                var pic = Image.FromFile(HttpContext.Request.MapPath(imagePath));
                pic.Dispose();
                return (true, imagePath);  // 上传成功，返回全路径 
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        #endregion

        #region 通用文件上传

        /// <summary>
        /// 通用文件上传
        /// </summary>
        /// <param name="fileInput">file表单参数名</param>
        /// <param name="folderName">上传目录的名称，如：~/upload/excels/teachers/...，则直接填写 excels/teachers</param>
        /// <returns></returns>
        public (bool, string) FileUpload(string fileInput, string folderName)
        {
            HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
            HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            HttpContext.Response.Charset = "UTF-8";
            try
            {
                if (string.IsNullOrEmpty(fileInput))
                    fileInput = "file";
                if (string.IsNullOrEmpty(folderName))
                    folderName = "files";
                var file = Request.Files[fileInput];
                if (file == null)
                    return (false, "上传文件为空！请勿非法提交");
                if (string.IsNullOrEmpty(file.ContentType) || (file.ContentType).IndexOf("image/", StringComparison.Ordinal) == -1)
                {
                    return (false, "未识别的文件格式！请勿非法提交");
                }
                var result = FileUploadPro(file, folderName);
                return result;
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        /// <summary>
        /// 通用文件上传
        /// </summary>
        /// <param name="file">待上传的文件</param>
        /// <param name="folderName">上传目录的名称，如：~/upload/excels/teachers/...，则直接填写 excels/teachers</param>
        /// <param name="extName">文件后缀</param>
        /// <returns></returns>
        public (bool, string) FileUploadPro(HttpPostedFileBase file, string folderName, string extName = "")
        {
            HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
            HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            HttpContext.Response.Charset = "UTF-8";
            if (file == null)
                return (false, "上传文件不能为空!");

            string fileName = Path.GetFileName(file.FileName); //文件名
            string fileExt = Path.GetExtension(fileName);  //文件后缀名

            if (fileExt == null)
                return (false, "无法识别的文件格式!!!");

            if (!string.IsNullOrEmpty(extName))
                if (!fileExt.ToLower().Equals(extName))
                    return (false, "上传文件格式错误!!");
            try
            {
                var dir = $"/upload/{folderName}/{DateTime.Now:yyyy}/{DateTime.Now:yyyyMM}/{DateTime.Now:yyyyMMdd}/";
                if (!Directory.Exists(HttpContext.Request.MapPath(dir)))
                    Directory.CreateDirectory(HttpContext.Request.MapPath(dir));
                string filePath = dir + Guid.NewGuid().ToString("N") + fileExt;  //全球唯一标示符，作为文件名
                file.SaveAs(HttpContext.Request.MapPath(filePath));
                return (true, filePath);  // 上传成功，返回全路径  
            }
            catch (Exception)
            {
                return (false, "上传文件失败");  // 上传成功，返回全路径  
            }
        }

        #endregion

        #region 从前台获取标签的值
        /// <summary>
        /// 从前台获取标签的值
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        protected string GetStringValueFromWeb(string tagName)
        {
            string strValue;

            if ((strValue = (Request.Form[tagName ?? ""] ?? "").Trim()) != "")
            {
                return strValue;
            }

            if ((strValue = (Request.Params[tagName ?? ""] ?? "").Trim()) != "")
            {
                return strValue;
            }

            if ((strValue = (Request.QueryString[tagName ?? ""] ?? "").Trim()) != "")
            {
                return strValue;
            }
            return "";
        }
        #endregion
    }
}