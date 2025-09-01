using BB.Core.Models;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BB.Web.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _db;  //local instance of the database service

        public List<Category> objCategoryList;  //our UI front end will support looping through and displaying Categories retrieved from the database and stored in a List

        public IndexModel(ApplicationDbContext db)  //dependency injection of the database service
        {
            _db = db;
            objCategoryList = new List<Category>();
        }

        public IActionResult OnGet()
        {
            objCategoryList = _db.Categories.ToList();
            return Page();
        }
    }
}
