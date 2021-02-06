using System;
using System.Collections.Generic;

namespace promotionengine.Order
{
    public interface IOrderService
    {
        public bool IsCartEmpty();
        public bool AddToCart (string id, int qty = 1);
        public OrderItem FindById (string id);
        public void EmptyCart();
        public int Checkout();
        public ICollection<OrderItem> GetCart();
    }
}
