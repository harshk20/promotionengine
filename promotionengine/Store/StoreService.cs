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
        }

        // For creating a store
        public bool OpenStore(string name)
        {
            try
            {
                if (name == null || name.Equals(string.Empty))
                    return false;

                this._currentStore = new Store(name);

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
