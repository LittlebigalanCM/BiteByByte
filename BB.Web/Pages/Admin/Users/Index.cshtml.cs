using BB.Application;
using BB.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(UnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public Dictionary<string, List<String>> UserRoles { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }

        public async Task OnGetAsync(bool success = false, string? message = null)
        {
            Success = success;
            Message = message;
            UserRoles = [];
            ApplicationUsers = [.. _unitOfWork.ApplicationUser.GetAll()];

            foreach (var user in ApplicationUsers)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                UserRoles[user.Id] = [.. userRole];
            }
        }

        public async Task<IActionResult> OnPostLockUnlock(string id)
        {
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if(user.LockoutEnd == null)
            {
                user.LockoutEnd = DateTime.Now.AddYears(100);
                user.LockoutEnabled = true;
            }

            else if(user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;
                user.LockoutEnabled = false;
            }

            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(100);
                user.LockoutEnabled = true;
            }

            _unitOfWork.ApplicationUser.Update(user);
            return RedirectToPage("./Index", new { success = true, message = "Update Successful" });
        }
    }
}
