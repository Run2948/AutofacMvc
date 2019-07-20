/* ==============================================================================
* 命名空间：Quick.Models.Common
* 类 名 称：ResultInfo
* 创 建 者：Qing
* 创建时间：2018-11-30 10:18:18
* CLR 版本：4.0.30319.42000
* 保存的文件名：ResultInfo
* 文件版本：V1.0.0.0
*
* 功能描述：N/A 
*
* 修改历史：
*
*
* ==============================================================================
*         CopyRight @ 班纳工作室 2018. All rights reserved
* ==============================================================================*/


namespace AutoFacMvc.Common.Models
{
    public class ResultInfo
    {
        public ResultInfo(int code, string msg, object data, string url)
        {
            Code = code;
            Msg = msg;
            Data = data;
            Url = url;
        }

        public ResultInfo(int code, string msg, object data)
        {
            Code = code;
            Msg = msg;
            Data = data;
        }

        public ResultInfo(int code, string msg, string url)
        {
            Code = code;
            Msg = msg;
            Url = url;
        }

        public ResultInfo(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }

        public ResultInfo()
        {
        }

        /// <summary>
        /// 返回的状态码：ok: 1, error: 0, timeout: 2
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回的提示信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回的数据对象
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 需要跳转的地址
        /// </summary>
        public string Url { get; set; }
    }
}
