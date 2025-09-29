using BB.Application;
using BB.Core.Models;
using BB.Core.Utilities;
using BB.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace BB.Web.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        public UnitOfWork _UnitOfWork;
        public SummaryModel(UnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        [BindProperty]
        public OrderDetailsCartVM OrderDetailsCart { get; set; }
        public void OnGet()
        {
            OrderDetailsCart = new OrderDetailsCartVM()
            {
                OrderHeader = new Core.Models.OrderHeader(),
                ListCart = new List<Core.Models.ShoppingCart>()
            };

            OrderDetailsCart.OrderHeader.OrderTotal = 0;

            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (claimsIdentity != null)
            {
                IEnumerable<ShoppingCart> cartList = _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId);

                if (cartList != null)
                {
                    OrderDetailsCart.ListCart = cartList.ToList();
                }

                foreach (var item in OrderDetailsCart.ListCart)
                {
                    item.MenuItem = _UnitOfWork.MenuItem.Get(u => u.Id == item.MenuItemId);
                    OrderDetailsCart.OrderHeader.OrderTotal += (item.MenuItem.Price * item.Count);
                }

                OrderDetailsCart.OrderHeader.OrderTotal += OrderDetailsCart.OrderHeader.OrderTotal * SD.SalesTaxPercent;
                ApplicationUser applicationUser = _UnitOfWork.ApplicationUser.Get(u => u.Id == userId);
                OrderDetailsCart.OrderHeader.DeliveryName = applicationUser.FullName;
                OrderDetailsCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
                OrderDetailsCart.OrderHeader.DeliveryDate = DateTime.Now.Date;
                OrderDetailsCart.OrderHeader.DeliveryTime = DateTime.Now;
            }
        }

        public IActionResult OnPost(string stripeToken)
        {
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            OrderDetailsCart.ListCart = _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).ToList();

            OrderDetailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            OrderDetailsCart.OrderHeader.OrderDate = DateTime.Now;
            OrderDetailsCart.OrderHeader.UserId = userId;
            OrderDetailsCart.OrderHeader.Status = SD.StatusSubmitted;
            OrderDetailsCart.OrderHeader.DeliveryTime = Convert.ToDateTime(OrderDetailsCart.OrderHeader.DeliveryDate.ToShortDateString() + " " + OrderDetailsCart.OrderHeader.DeliveryTime.ToShortTimeString());
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            _UnitOfWork.OrderHeader.Add(OrderDetailsCart.OrderHeader);

            foreach(var item in OrderDetailsCart.ListCart)
            {
                item.MenuItem = _UnitOfWork.MenuItem.Get(u => u.Id == item.MenuItemId);
                OrderDetails OrderDetailsObj = new OrderDetails()
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = OrderDetailsCart.OrderHeader.Id,
                    Price = item.MenuItem.Price,
                    Count = item.Count,
                };
                OrderDetailsCart.OrderHeader.OrderTotal += (OrderDetailsObj.Price * (1 + SD.SalesTaxPercent));
                _UnitOfWork.OrderDetails.Add(OrderDetailsObj);
            }
            OrderDetailsCart.OrderHeader.OrderTotal = Convert.ToDouble(String.Format("{0:.##}", OrderDetailsCart.OrderHeader.OrderTotal));
            
            if(stripeToken != null)
            {
                var options = new Stripe.ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(OrderDetailsCart.OrderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Order ID: " + OrderDetailsCart.OrderHeader.Id,
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                OrderDetailsCart.OrderHeader.TransactionId = charge.Id;

                if(charge.Status.ToLower() == "succeeded")
                {
                    OrderDetailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                }
                else
                {
                    OrderDetailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
                _UnitOfWork.OrderHeader.Update(OrderDetailsCart.OrderHeader);

            }
            return RedirectToPage("/Customer/Cart/OrderConfirmation", new { id = OrderDetailsCart.OrderHeader.Id } );
        }
    }
}
