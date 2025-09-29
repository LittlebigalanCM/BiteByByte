using BB.Application;
using BB.Core.Models;
using BB.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BB.Web.Pages.Customer.Cart
{
    public class OrderConfirmationModel : PageModel
    {
        UnitOfWork _UnitOfWork;
        public OrderConfirmationModel(UnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        [BindProperty]
        public int OrderId { get; set; }
        public void OnGet(int orderId)
        {
            OrderId = orderId;
            IEnumerable<ShoppingCart> ObjShoppingCart;
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            ObjShoppingCart = _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId);

            _UnitOfWork.ShoppingCart.Delete(ObjShoppingCart);

            HttpContext.Session.SetInt32(SD.ShoppingCartCount, 0);
        }
    }
}
