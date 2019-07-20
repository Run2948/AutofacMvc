using System.Linq;
using AutoFacMvc.Common.Extensions;
using AutoFacMvc.Common.Models;
using AutoFacMvc.Models;
using AutoFacMvc.Repository.Core;
using AutoFacMvc.Repository.Interface;

namespace AutoFacMvc.Repository
{
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        private readonly SchoolContext _context;

        public UserRepository(SchoolContext context)
            : base(context)
        {
            _context = context;
        }

        public SessionInfo Login(string userName, string password)
        {
            password = password.EncryptionWithSalt(userName);
            var loginUser = _context.UserInfos.FirstOrDefault(l => l.UserName == userName && l.Password == password);
            return loginUser?.Mapper<SessionInfo>();
        }
    }
}