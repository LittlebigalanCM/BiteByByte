using BB.Core.Models;

namespace BB.Web.ViewModels
{
    public class OrderDetailsCartVM
    {
        public OrderHeader? OrderHeader { get; set; }
        public List<ShoppingCart>? ListCart { get; set; }
    }
}
