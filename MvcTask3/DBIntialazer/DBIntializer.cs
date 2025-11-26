using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MvcTask3.Models;

namespace MvcTask3.DBIntialazer
{
    public class DBIntializer : IdBIntializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DBIntializer> _logger;

        public DBIntializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<DBIntializer> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public void Intialize()
        {
            try
            {
                // Roles
                CreateRoleIfNotExists("SuperAdmin");
                CreateRoleIfNotExists("Admin");
                CreateRoleIfNotExists("Customer");
                CreateRoleIfNotExists("Employee");

                // SuperAdmin User
                var superAdminEmail = "superadmin@gmail.com";
                var superAdminUserName = "SuperAdmin";

                var superAdmin = _userManager.FindByEmailAsync(superAdminEmail).Result;
                if (superAdmin == null)
                {
                    superAdmin = new ApplicationUser
                    {
                        UserName = superAdminUserName,
                        Email = superAdminEmail,
                        EmailConfirmed = true,
                        FirstName = "Super",
                        LastName = "Admin"
                    };
                    _userManager.CreateAsync(superAdmin, "Admin12345").GetAwaiter().GetResult();
                }

                // Assign SuperAdmin Role
                if (!_userManager.IsInRoleAsync(superAdmin, "SuperAdmin").Result)
                {
                    _userManager.AddToRoleAsync(superAdmin, "SuperAdmin").GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DBInitializer Error: {ex.Message}");
            }
        }

        private void CreateRoleIfNotExists(string roleName)
        {
            if (!_roleManager.RoleExistsAsync(roleName).Result)
            {
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
        }
    }
}
