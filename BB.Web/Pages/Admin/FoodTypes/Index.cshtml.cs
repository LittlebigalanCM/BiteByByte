using BB.Core.Models;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.FoodTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public List<FoodType> objFoodTypeList;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
            objFoodTypeList = new List<FoodType>();
        }

        public IActionResult OnGet()
        {
            objFoodTypeList = _db.FoodTypes.ToList();
            return Page();
        }
    }
}
