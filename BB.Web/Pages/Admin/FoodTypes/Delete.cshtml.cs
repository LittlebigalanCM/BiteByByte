using BB.Application;
using BB.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.FoodTypes
{
    public class DeleteModel : PageModel
    {
        private readonly UnitOfWork _UnitOfWork;

        [BindProperty]
        public FoodType ObjFoodType { get; set; } = new FoodType();

        public DeleteModel(UnitOfWork UnitOfWork)
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
            _UnitOfWork.FoodType.Delete(ObjFoodType);

            return RedirectToPage("./Index");
        }
    }
}
