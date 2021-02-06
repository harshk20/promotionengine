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
                this._inventoryService.GenerateDefaultSKU();
                this._promotionService.GenerateDefaultPromotion();

                Console.Clear();
                Console.WriteLine("---------------- Welcome to the " + name + " store ----------------");

                Console.WriteLine("Here is the list of Items that you can buy :");
                foreach (var item in this._inventoryService.GetStockItems())
                {
                    Console.Write(item.Id + "  " + item.Currency + item.Price + "  ");
                }

                Console.WriteLine("\nHere are some offers going on :");
                foreach (var promo in this._promotionService.GetPromotionOffers())
                {
                    if (promo.OfferType == OfferType.BUY_N_ITEMS_FOR_FIXED)
                        Console.WriteLine("Buy "+ promo.OfferItems.First().MinQuantity + " " +
                                           promo.OfferItems.First().Id + " for fixed price of " +
                                           promo.FixedPrice);
                    else if (promo.OfferType == OfferType.BUY_COMBINED_ITEMS_FOR_FIXED)
                    {
                        Console.Write("Buy ");
                        foreach(var offerItem in promo.OfferItems)
                        {
                            Console.Write(offerItem.MinQuantity + offerItem.Id + " ");
                        }
                        Console.Write("for fixed price of " + promo.FixedPrice + "\n");

                    }
                }

                Console.WriteLine("Items in your Cart  :");
                foreach (var orderItem in this._orderService.GetCart())
                {
                    Console.WriteLine(orderItem.Id + " " + orderItem.Quantity);
                }


                this._inventoryService.GenerateDefaultSKU();
                this._promotionService.GenerateDefaultPromotion();
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
