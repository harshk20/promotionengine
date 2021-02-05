using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace promotionengine.Order
{
    public class OrderService : IOrderService
    {
        private readonly ICollection<OrderItem> _cart;

        public OrderService()
        {
            this._cart = new Collection<OrderItem>();
        }

        public bool AddToCart(string id, int qty = 1)
        {
            try
            {
                // First we have to check if the Stock item actually exists with this id

                OrderItem orderItem;
                if (!this._cart.Any(oi => oi.Id.Equals(id)))
                {
                    orderItem = new OrderItem(id, qty);
                    this._cart.Add(orderItem);
                }
                else
                {
                    orderItem = this._cart.Where(oi => oi.Id.Equals(id)).Single();
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
            throw new NotImplementedException();
        }

        public int Checkout()
        {
            throw new NotImplementedException();
        }

        public bool IsCartEmpty()
        {
            return this.Cart.Count == 0;
        }

        public ICollection<OrderItem> Cart { get { return this._cart; } }

    }
}
