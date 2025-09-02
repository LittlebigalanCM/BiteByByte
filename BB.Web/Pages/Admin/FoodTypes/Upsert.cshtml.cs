using BB.Core.Models;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.FoodTypes
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public FoodType objFoodType { get; set; }

        public UpsertModel(ApplicationDbContext db)
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
            if (objFoodType.Id == 0)
            {
                _db.FoodTypes.Add(objFoodType);
            }
            else
            {
                _db.FoodTypes.Update(objFoodType);
            }
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
