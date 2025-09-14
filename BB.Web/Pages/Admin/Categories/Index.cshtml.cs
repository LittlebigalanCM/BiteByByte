using BB.Core.Models;
using BB.Application;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BB.Web.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {

        private readonly UnitOfWork _UnitOfWork;  //access to Generic Repository via UnitOfWork

        public List<Category> ObjCategoryList { get; set; } = [];  //our UI front end will support looping through and displaying Categories retrieved from the database and stored in a List

        public IndexModel(UnitOfWork UnitOfWork)  //dependency injection of the database service
        {
            _UnitOfWork = UnitOfWork;
            ObjCategoryList = new List<Category>();
        }

        public IActionResult OnGet()
        {
            ObjCategoryList = [.. _UnitOfWork.Category.GetAll()]; //retrieve all Categories from the database via the Generic Repository
            return Page();
        }
    }
}
