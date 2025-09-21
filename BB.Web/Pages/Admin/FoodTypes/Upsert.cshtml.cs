using BB.Application;
using BB.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.FoodTypes
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _UnitOfWork;

        [BindProperty]
        public FoodType ObjFoodType { get; set; } = new FoodType();

        public UpsertModel(UnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                ObjFoodType = _UnitOfWork.FoodType.GetById(id) ?? new FoodType();
            }

            if (ObjFoodType == null)
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
            if (ObjFoodType.Id == 0)
            {
                _UnitOfWork.FoodType.Add(ObjFoodType);
            }
            else
            {
                _UnitOfWork.FoodType.Update(ObjFoodType);
            }
            return RedirectToPage("./Index");
        }
    }
}
