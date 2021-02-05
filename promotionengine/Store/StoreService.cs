using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using promotionengine.Inventory;
using promotionengine.Order;
using promotionengine.Promotion;

namespace promotionengine.Store
{
    public class StoreService : IStoreService
    {
        private readonly ICollection<Store> _stores;    // member to hold many stores
        private Store _currentStore;                    // member for operating on current store


        private readonly IInventoryService _inventoryService;
        private readonly IPromotionService _promotionService;
        private readonly IOrderService _orderService;

        public StoreService(IInventoryService inventoryService,
                            IPromotionService promotionService,
                            IOrderService orderService)
        {
            this._inventoryService = inventoryService;
            this._promotionService = promotionService;
            this._orderService = orderService;
            this._stores = new Collection<Store>();
        }

        // For creating a store
        public bool CreateStore(string name)
        {
            try
            {
                if (_stores.Any(s => s.Name == name))
                    return false;

                _stores.Add(new Store(name));
                return true;
            } catch(Exception ex)
            {
                _ = ex;
                return false;
            }
        }

        // For running a particular store
        public bool RunStore(string name)
        {
            try
            {
                var store = _stores.First(s => s.Name == name);

                if (store == null)
                    return false;

                this._currentStore = store;

                this._inventoryService.CreateSKU("A", 10, "$");
                this._promotionService.CreatePromotion(OfferType.BUY_N_ITEMS_FOR_FIXED, new List<OfferItem> { new OfferItem("A", 2) }, 10);
                this._orderService.AddToCart("A", 3);
                var total = this._orderService.Checkout();

                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }
    }
}
