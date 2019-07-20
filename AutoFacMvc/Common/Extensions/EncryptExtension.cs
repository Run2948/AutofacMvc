using System.Security.Cryptography;
using System.Text;

namespace AutoFacMvc.Common.Extensions
{
    /// <summary>
    /// 常用的加密解密算法
    /// </summary>
    public static class EncryptExtension
    {
        #region MD5加密

        /// <summary>
        /// 用hash方法产生md5值
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string ToMd5(this string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(strText));
            var result = new StringBuilder();
            foreach (var t in buffer)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        #endregion

        #region 带盐值加密

        /// <summary>
        /// 先加盐后加密
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="userName"></param>
        public static string EncryptionWithSalt(this string password, string userName)
        {
            var tmpMd5 = userName + password.ToMd5() + userName;
            return tmpMd5.ToMd5();
        }

        #endregion
    }
}