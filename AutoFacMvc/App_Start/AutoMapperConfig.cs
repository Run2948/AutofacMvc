using AutoFacMvc.Common.Extensions;
using AutoFacMvc.Common.Models;
using AutoFacMvc.Models;
using AutoMapper;

namespace AutoFacMvc
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<UserInfo, SessionInfo>();
                m.CreateMap<SessionInfo, UserInfo>().IgnoreAllNonExisting();
            });
        }
    }
}