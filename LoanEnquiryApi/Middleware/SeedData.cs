using LoanEnquiryApi.Constant;
using LoanEnquiryApi.Entity;
using Microsoft.AspNetCore.Identity;

namespace LoanEnquiryApi.Middleware
{
    public static class SeedData
    {
        public static void Initialize(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;

            var context = service.GetRequiredService<DataContext>();
            var userManager = service.GetRequiredService<UserManager<UserEntity>>();
            var roleManager = service.GetRequiredService<RoleManager<UserRoleEntity>>();

            // Initialize User Role
            var initialRoles = new List<UserRoleEntity>()
            {
                new UserRoleEntity() { Id = "5c2f50d4-c2c0-48b8-9ae7-e9207d6bfffa", Name = UserRole.Admin.ToString(), NormalizedName = UserRole.Admin.ToString() },
            };

            var userRoleEntities = roleManager.Roles.ToList();

            var newUserRoles = initialRoles.Where(i => !userRoleEntities.Any(u => u.Name == i.Name));

            foreach (var userRoleEntity in newUserRoles)
            {
                roleManager.CreateAsync(userRoleEntity).GetAwaiter().GetResult();
            }

            // Initialize User Manager
            if (!userManager.Users.Any(u => u.UserName == "superuser"))
            {
                var superuser = new UserEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = "superuser",
                    UserName = "superuser",
                    Email = "superuser@email.com",
                    NormalizedUserName = "superuser",
                    UserRole = UserRole.Admin.ToString(),
                };

                userManager.CreateAsync(superuser, "superuser123#").GetAwaiter().GetResult();
            }

            if (!userManager.Users.Any(u => u.UserName == "admin"))
            {
                var adminuser = new UserEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = "admin",
                    UserName = "admin",
                    Email = "admin@email.com",
                    NormalizedUserName = "admin",
                    UserRole = UserRole.Admin.ToString()
                };

                userManager.CreateAsync(adminuser, "admin123#").GetAwaiter().GetResult();
            }


            context.SaveChanges();
        }
    }
}
