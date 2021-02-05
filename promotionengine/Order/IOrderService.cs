using System;
namespace promotionengine.Order
{
    public interface IOrderService
    {
        public bool IsCartEmpty();
        public bool AddToCart (string id, int qty = 1);
        public int Checkout();
    }
}
