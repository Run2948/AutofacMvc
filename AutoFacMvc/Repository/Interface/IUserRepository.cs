/* ==============================================================================
* 命名空间：AutoFacMvc.Repository 
* 类 名 称：IUserRepository
* 创 建 者：Qing
* 创建时间：2019/07/20 15:55:15
* CLR 版本：4.0.30319.42000
* 保存的文件名：IUserRepository
* 文件版本：V1.0.0.0
*
* 功能描述：N/A 
*
* 修改历史：
*
*
* ==============================================================================
*         CopyRight @ 班纳工作室 2019. All rights reserved
* ==============================================================================*/

using AutoFacMvc.Common.Models;
using AutoFacMvc.Models;
using AutoFacMvc.Repository.Core;

namespace AutoFacMvc.Repository.Interface
{
    public interface IUserRepository : IRepository<UserInfo>
    {
        SessionInfo Login(string userName, string password);
    }
}
