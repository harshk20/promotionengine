﻿using System;
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
            this._stockItems = new Collection<SKU>();
        }

        /**
         * Creating SKUs in the inventory
         */
        public bool CreateSKU(string id, int price, string currency)
        {
            try
            {
                if (this._stockItems.Any(si => si.Id.Equals(id)))
                    return false;

                this._stockItems.Add(new SKU(id, price, currency));
                return true;

            } catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }

        /**
         * Method to get the SKU via id
         */
        public SKU GetSKUById(string id)
        {
            try
            {
                if (!this._stockItems.Any(si => si.Id.Equals(id)))
                    return null;

                return this._stockItems.Where(si => si.Id.Equals(id)).Single();
            }
            catch (Exception ex)
            {
                _ = ex;
                return null;
            }
        }

        public void GenerateDefaultSKU()
        {

            CreateSKU("A", 50, "$");
            CreateSKU("B", 30, "$");
            CreateSKU("C", 20, "$");
            CreateSKU("D", 15, "$");
            CreateSKU("E", 25, "$");
            CreateSKU("F", 5, "$");
            CreateSKU("G", 10, "$");
            CreateSKU("H", 1, "$");

        }

        public ICollection<SKU> GetStockItems()
        {
            return this.StockItems;
        }

        public ICollection<SKU> StockItems { get { return this._stockItems; } }

    }
}
