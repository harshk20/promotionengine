using System;
namespace promotionengine.Order
{
    /**
     * Class to manage orderItems
     */
    public class OrderItem
    {
        private string _id;          // Member to hold item name
        private int _quantity;       // Member to hold the quantity of stock item
        
        // Constructor
        public OrderItem(string id, int qty)
        {
            this._id = id;
            this._quantity = qty;
        }

        public void AddQuantity (int qty)
        {
            this._quantity += qty;
        }

        public void RemoveQuantity (int qty)
        {
            this._quantity -= qty;
        }

        public string Id { get { return this._id; }  }
        
        public int Price { get; set; }          // Member to hold the unit price of item
        public string Currency { get; set; }    // Member to hold the currency of price
        public int Amount { get; set; }         // Member to hold the amount = quantity*price
        public int OfferAmount { get; set; }    // Member to hold calculated amount after applying the promotion
        public bool IsOfferApplied { get; set; }
    }
}
