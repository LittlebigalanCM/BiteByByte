using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.Roles
{
    public class IndexModel(RoleManager<IdentityRole> roleManager) : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public IEnumerable<IdentityRole> RolesObj { get; set; } = Enumerable.Empty<IdentityRole>();
        public bool Success { get; set; }
        public string? Message { get; set; }

        public void OnGet(bool success = false, string? message = null)
        {
            Success = success;
            Message = message;
            RolesObj = _roleManager.Roles;
        }
    }
}