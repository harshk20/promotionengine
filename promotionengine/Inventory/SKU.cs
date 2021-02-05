using System;
namespace promotionengine.Inventory
{
    /**
     * Class to manage stock items
     */
    public class SKU
    {
        private readonly string _id;          // Member to hold item name
        private readonly int _price;          // Member to hold item's unit price
        private readonly string _currency;    // Member to hold the currency of unit price

        // Constructor
        public SKU(string id, int price, string currency)
        {
            this._id = id;
            this._price = price;
            this._currency = currency;
        }

        public string Id { get { return this._id; } }
        public int Price { get { return this._price; } }
        public string Currency { get { return this._currency; } }

    }
}
