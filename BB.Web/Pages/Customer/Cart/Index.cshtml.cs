using BB.Application;
using BB.Core.Models;
using BB.Core.Utilities;
using BB.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BB.Web.Pages.Customer.Cart
{
    public class IndexModel : PageModel
    {
        public readonly UnitOfWork _UnitOfWork;
        public IndexModel(UnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public OrderDetailsCartVM OrderDetailsCart { get; set; }
        public IEnumerable<MenuItem> MenuItemList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }

        [BindProperty]
        public MenuItem ObjMenuItem { get; set; }

        [BindProperty]
        public ShoppingCart ObjShoppingCart { get; set; }

        [BindProperty]
        public int TxtCount { get; set; }

        public void OnGet()
        {
            OrderDetailsCart = new OrderDetailsCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = new List<ShoppingCart>()
            };

            OrderDetailsCart.OrderHeader.OrderTotal = 0;

            MenuItemList = _UnitOfWork.MenuItem.GetAll(null, null, "Category,FoodType").ToList();
            CategoryList = _UnitOfWork.Category.GetAll(null, c => c.DisplayOrder, null).ToList();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                var cnt = _UnitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).Count();
                HttpContext.Session.SetInt32(SD.ShoppingCartCount, cnt);
                try
                {
                    OrderDetailsCart.ListCart = _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList();
                }
                catch (InvalidOperationException)
                {
                    OrderDetailsCart.ListCart = null;
                }

                if(OrderDetailsCart.ListCart != null)
                {
                    foreach (var item in OrderDetailsCart.ListCart)
                    {
                        item.MenuItem = _UnitOfWork.MenuItem.Get(m => m.Id == item.MenuItemId);
                        OrderDetailsCart.OrderHeader.OrderTotal += (item.MenuItem.Price * item.Count);
                    }
                }
            }
            else
            {
                var cnt = 0;
                HttpContext.Session.SetInt32(SD.ShoppingCartCount, cnt);
            }
        }


        public IActionResult OnPost()
        {
            // Check to see if the user is signed in
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ShoppingCart? existCart;
            try
            {
                existCart = _UnitOfWork.ShoppingCart.Get(s =>
                    s.ApplicationUserId == userId && s.MenuItemId == ObjMenuItem.Id);
            }
            catch (InvalidOperationException)
            {
                existCart = null;
            }

            if (existCart == null)
            {
                ObjShoppingCart = new ShoppingCart()
                {
                    ApplicationUserId = userId,
                    MenuItemId = ObjMenuItem.Id,
                    Count = TxtCount
                };

                _UnitOfWork.ShoppingCart.Add(ObjShoppingCart);

                var cnt = _UnitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == ObjShoppingCart.ApplicationUserId)
                    .Count();
                HttpContext.Session.SetInt32(SD.ShoppingCartCount, cnt);
            }
            else
            {
                existCart.Count += TxtCount;
                _UnitOfWork.ShoppingCart.Update(existCart);

                var cnt = _UnitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == existCart.ApplicationUserId)
                    .Count();
                HttpContext.Session.SetInt32(SD.ShoppingCartCount, cnt);
            }

            return RedirectToPage("Index");
        }


        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _UnitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cart.Count += 1;
            _UnitOfWork.ShoppingCart.Update(cart);
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _UnitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cart.Count == 1)
            {
                _UnitOfWork.ShoppingCart.Delete(cart);
                var cnt = _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).Count();
                HttpContext.Session.SetInt32(SD.ShoppingCartCount, cnt);
            }
            else
            {
                cart.Count -= 1;
                _UnitOfWork.ShoppingCart.Update(cart);
            }
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _UnitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cart != null)
            {
                _UnitOfWork.ShoppingCart.Delete(cart);
                var cnt = _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).Count();
                HttpContext.Session.SetInt32(SD.ShoppingCartCount, cnt);
            }
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}
