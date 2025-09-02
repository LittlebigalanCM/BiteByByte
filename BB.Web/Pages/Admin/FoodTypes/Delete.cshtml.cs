using BB.Core.Models;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.FoodTypes
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public FoodType objFoodType { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
            objFoodType = new FoodType();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                objFoodType = _db.FoodTypes.FirstOrDefault(c => c.Id == id) ?? new FoodType();
            }

            if (objFoodType == null)
            {
                return NotFound();
            }

            return Page();
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _db.FoodTypes.Remove(objFoodType);
            _db.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
