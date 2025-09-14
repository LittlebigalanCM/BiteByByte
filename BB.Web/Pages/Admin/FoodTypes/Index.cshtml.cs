using BB.Application;
using BB.Core.Models;
using BB.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Admin.FoodTypes
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _UnitOfWork;

        public List<FoodType> ObjFoodTypeList { get; set; } = [];

        public IndexModel(UnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult OnGet()
        {
            ObjFoodTypeList = [.. _UnitOfWork.FoodType.GetAll()];
            return Page();
        }
    }
}
