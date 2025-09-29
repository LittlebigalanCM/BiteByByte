using BB.Application;
using BB.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _UnitOfWork;
        public IndexModel(UnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public List<MenuItem> MenuItemList { get; set; }
        public List<Category> CategoryList { get; set; }
        public void OnGet()
        {
            MenuItemList = _UnitOfWork.MenuItem.GetAll(null, null, "Category,FoodType").ToList();
            CategoryList = _UnitOfWork.Category.GetAll(null, c => c.DisplayOrder, null).ToList();
        }
    }
}
