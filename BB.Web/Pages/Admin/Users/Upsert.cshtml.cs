using BB.Application;
using BB.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.Users
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UpsertModel(UnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)

        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public ApplicationUser AppUser { get; set; }
        public List<string> UsersRoles { get; set; } //Roles Assigned
        public List<string> AllRoles { get; set; }   //All Roles Possible
        public List<string> OldRoles { get; set; }   //Roles Assigned when Model Loads

        public async Task OnGetAsync(string id)
        {
            AppUser = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            var roles = await _userManager.GetRolesAsync(AppUser);
            UsersRoles = [.. roles];
            OldRoles = [.. roles];
            AllRoles = [.. _roleManager.Roles.Select(r => r.Name)];
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newRoles = Request.Form["Roles"].ToList();
            UsersRoles = [.. newRoles.Where(r => r!=null).Select(r => r!)];
            var OldRoles = await _userManager.GetRolesAsync(AppUser);
            var rolesToAdd = new List<string>();
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == AppUser.Id);

            user.FirstName = AppUser.FirstName;
            user.LastName = AppUser.LastName;
            user.Email = AppUser.Email;
            user.PhoneNumber = AppUser.PhoneNumber;
            _unitOfWork.ApplicationUser.Update(user);

            foreach(var r in UsersRoles)
            {
                if(!OldRoles.Contains(r)) 
                    rolesToAdd.Add(r);
            }

            foreach (var r in OldRoles)
            {
                if (!UsersRoles.Contains(r))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, r);
                }
            }

            var result1 = await _userManager.AddToRolesAsync(user, rolesToAdd.AsEnumerable());
            return RedirectToPage("./Index", new { success = true, message = "Update Successful" });
        }
    }
}
