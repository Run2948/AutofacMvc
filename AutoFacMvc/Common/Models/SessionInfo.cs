using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Common.Models
{
    public class SystemKeys
    {
        public const string UserSession = "UserInfo";

        public const string UserLogin = "/Account/Login";
    }

    public class SessionInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
    }
}