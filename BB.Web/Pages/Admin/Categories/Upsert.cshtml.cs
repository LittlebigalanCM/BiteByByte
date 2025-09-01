using BB.Core.Models;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BB.Web.Pages.Admin.Categories
{
    public class UpsertModel : PageModel
    {

        private readonly ApplicationDbContext _db;  //local instance of the database service

        [BindProperty]
        public Category objCategory { get; set; }  //our UI front end will deal with a single Category Object (whether creating or updating.  The [BindProperty] maintains automatic synchronization between the UI and backend code.

        public UpsertModel(ApplicationDbContext db)  //dependency injection of the database service
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
            //If New
            if (objCategory.Id == 0)
            {
                _db.Categories.Add(objCategory);
            }
            //existing
            else
            {
                _db.Categories.Update(objCategory); //memory only
            }
            _db.SaveChanges(); //saves changes to database
            return RedirectToPage("./Index");
        }
    }
}
