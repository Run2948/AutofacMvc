using System.Linq;
using AutoFacMvc.Common.Extensions;
using AutoFacMvc.Models;

namespace AutoFacMvc.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SchoolContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            if (!context.Students.Any())
            {
                context.Students.AddOrUpdate(
                    s => s.Id,
                    new Student { Name = "Andrew Peters", Age = 18 },
                    new Student { Name = "Brice Lambson", Age = 29 },
                    new Student { Name = "Rowan Miller", Age = 56 });
            }

            if (!context.UserInfos.Any())
            {
                context.UserInfos.AddOrUpdate(
                    s => s.Id,
                    new UserInfo { UserName = "admin", Password = "12345678LYY".EncryptionWithSalt("admin"), RealName = "Admin" }
                    );
            }
        }
    }
}
