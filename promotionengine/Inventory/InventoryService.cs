using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace promotionengine.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly ICollection<SKU> _stockItems;

        public InventoryService()
        {
            _stockItems = new Collection<SKU>();
        }

        /**
         * Creating SKUs in the inventory
         */
        public bool CreateSKU(string id, int price, string currency)
        {
            if (_stockItems.Any(si => si.Id.Equals(id)))
                return false;

            _stockItems.Add(new SKU(id, price, currency));
            return true;
        }

        public ICollection<SKU> StockItems { get { return this._stockItems; } }

    }
}
