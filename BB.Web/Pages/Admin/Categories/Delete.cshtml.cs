using BB.Core.Models;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BB.Web.Pages.Admin.Categories
{
    public class DeleteModel : PageModel
    {

        private readonly ApplicationDbContext _db;  //local instance of the database service

        [BindProperty]
        public Category objCategory { get; set; }  //our UI front end will deal with a single Category Object (whether creating or updating.  The [BindProperty] maintains automatic synchronization between the UI and backend code.

        public DeleteModel(ApplicationDbContext db)  //dependency injection of the database service
        {
            _db = db;
            objCategory = new Category();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null && id != 0) // we are in edit mode of existing category
            {
                objCategory = _db.Categories.FirstOrDefault(c => c.Id == id) ?? new Category(); // Use null-coalescing operator to handle null
            }

            if (objCategory == null)
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
            _db.Categories.Remove(objCategory);  //Removes from memory
            _db.SaveChanges();   //saves to DB

            return RedirectToPage("./Index");
        }
    }
}
