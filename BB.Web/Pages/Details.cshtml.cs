using BB.Application;
using BB.Core.Models;
using BB.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BB.Web.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly UnitOfWork _UnitOfWork;
        public DetailsModel(UnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        [BindProperty]
        public MenuItem ObjMenuItem { get; set; }
        [BindProperty]
        public int TxtCount { get; set; } = 1;
        public ShoppingCart ObjShoppingCart { get; set; }
        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                ObjMenuItem = _UnitOfWork.MenuItem.Get(m => m.Id == id, false, "Category,FoodType");

                var claimsIdentity = User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                HttpContext.Session.SetString("UserLoggedIn", claim?.Value ?? string.Empty);
            }
            else
            {
                throw new Exception("Menu Item Not Found");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ShoppingCart? existCart;
            try
            {
                existCart = _UnitOfWork.ShoppingCart.Get(s => s.ApplicationUserId == userId && s.MenuItemId == ObjMenuItem.Id);
            }
            catch (InvalidOperationException)
            {
                existCart = null;
            }

            if(existCart == null)
            {
                ObjShoppingCart = new ShoppingCart()
                {
                    ApplicationUserId = userId,
                    MenuItemId = ObjMenuItem.Id,
                    Count = TxtCount
                };
                _UnitOfWork.ShoppingCart.Add(ObjShoppingCart);
            }
            else
            {
                existCart.Count += TxtCount;
                _UnitOfWork.ShoppingCart.Update(existCart);
            }
            var cnt = _UnitOfWork.ShoppingCart.GetAll(U => U.ApplicationUserId == ObjShoppingCart.ApplicationUserId).Count();
            HttpContext.Session.SetInt32(SD.ShoppingCartCount, cnt);
            return RedirectToPage("Index");
        }
    }
}
