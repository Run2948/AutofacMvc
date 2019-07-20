using System.Data.Entity;

namespace AutoFacMvc.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

    }

}
