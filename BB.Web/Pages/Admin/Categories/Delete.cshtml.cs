using BB.Application;
using BB.Core.Models;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BB.Web.Pages.Admin.Categories
{
    public class DeleteModel : PageModel
    {

        private readonly UnitOfWork _UnitOfWork;  //local instance of the database service

        [BindProperty]
        public Category ObjCategory { get; set; } = new Category(); //our UI front end will deal with a single Category Object (whether creating or updating.  The [BindProperty] maintains automatic synchronization between the UI and backend code.

        public DeleteModel(UnitOfWork UnitOfWork)  //dependency injection of the database service
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null && id != 0) // we are in edit mode of existing category
            {
                ObjCategory = _UnitOfWork.Category.GetById(id) ?? new Category(); // Use null-coalescing operator to handle null
            }

            if (ObjCategory == null)
            {
                return NotFound();
            }

            return Page(); //assume insert new mode
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _UnitOfWork.Category.Delete(ObjCategory);  //Removes from memory

            return RedirectToPage("./Index");
        }
    }
}
