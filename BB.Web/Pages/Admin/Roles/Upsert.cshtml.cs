using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.Roles
{
    public class UpsertModel(RoleManager<IdentityRole> roleManager) : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        [BindProperty]
        public IdentityRole CurrentRole { get; set; } = new IdentityRole();
        [BindProperty]
        public bool IsUpdate { get; set; }

        public async Task OnGetAsync(string? id)
        {
            if (id != null)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    CurrentRole = role;
                    IsUpdate = true;
                }
                else
                {
                    CurrentRole = new IdentityRole();
                    IsUpdate = false;
                }
            }
            else
            {
                CurrentRole = new IdentityRole();
                IsUpdate = false;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!IsUpdate)
            {
                CurrentRole.NormalizedName = CurrentRole.Name?.ToUpper();
                var result = await _roleManager.CreateAsync(CurrentRole);
                if (result.Succeeded)
                {
                    return RedirectToPage("./Index", new { success = true, message = "Successfully Added" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(CurrentRole.Id);
                if (existingRole == null)
                {
                    return NotFound(); // Safety net
                }

                if (!string.Equals(existingRole.Name, CurrentRole.Name, StringComparison.OrdinalIgnoreCase))
                {
                    existingRole.Name = CurrentRole.Name;
                    existingRole.NormalizedName = CurrentRole.Name?.ToUpper();

                    var result = await _roleManager.UpdateAsync(existingRole);
                    if (result.Succeeded)
                    {
                        return RedirectToPage("./Index", new { success = true, message = "Update Successful" });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    // No changes detected
                    return RedirectToPage("./Index", new { success = true, message = "No changes detected." });
                }

                return Page();
            }
        }
    }
}


