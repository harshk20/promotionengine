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

        private void PrintStore()
        {
            Console.Clear();
            Console.WriteLine("---------------- Welcome to the " + this._currentStore.Name + " store ----------------");

            Console.WriteLine("Here is the list of Items that you can buy :");
            foreach (var item in this._inventoryService.GetStockItems())
            {
                Console.Write(item.Id + "  " + item.Currency + item.Price + "  ");
            }

            Console.WriteLine("\nHere are some offers going on :");
            foreach (var promo in this._promotionService.GetPromotionOffers())
            {
                if (promo.OfferType == OfferType.BUY_N_ITEMS_FOR_FIXED)
                    Console.WriteLine("Buy " + promo.OfferItems.First().MinQuantity + " " +
                                       promo.OfferItems.First().Id + " for fixed price of " +
                                       promo.FixedPrice);
                else if (promo.OfferType == OfferType.BUY_COMBINED_ITEMS_FOR_FIXED)
                {
                    Console.Write("Buy ");
                    foreach (var offerItem in promo.OfferItems)
                    {
                        Console.Write(offerItem.MinQuantity + offerItem.Id + " ");
                    }
                    Console.Write("for fixed price of " + promo.FixedPrice + "\n");

                }
            }

        }

        private bool AddToCart()
        {
            Console.WriteLine("\n'Esc' for MainMenu");
            Console.WriteLine("'Any' for Input");
            Console.WriteLine("Item Quanity");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                var line = Console.ReadLine();
                var itemAndQty = line.Split(" ");
                var itemId = itemAndQty[0];
                int itemQty = 1;
                if (itemAndQty.Count() > 1)
                    int.TryParse(itemAndQty[1], out itemQty);

                this._orderService.AddToCart(itemId, itemQty);

            }

            return false;
        }

        private bool NewSKU()
        {
            Console.WriteLine("\n'Esc' for MainMenu");
            Console.WriteLine("'Any' for Input");
            Console.WriteLine("Item Price");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                var line = Console.ReadLine();
                var itemAndPrice = line.Split(" ");
                var itemId = itemAndPrice[0];
                int itemPrice = 1;
                if (itemAndPrice.Count() > 1)
                    int.TryParse(itemAndPrice[1], out itemPrice);

                this._inventoryService.CreateSKU(itemId, itemPrice, "$");

            }

            return false;
        }

        private bool ClearCart()
        {
            this._orderService.EmptyCart();
            return false;
        }


        private bool NewPromotion()
        {
            Console.WriteLine("\n'Esc' for MainMenu");
            Console.WriteLine("'1' for Buy 'n' Item for fixed price (N ITEM PRICE) ");
            Console.WriteLine("'2' for Buy combined Items for fixed price (ITEM ITEM ITEM ... PRICE)");
            bool mainMenu = false;
            while (!mainMenu)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    mainMenu = true;
                }
                else if (key.Key == ConsoleKey.D1)
                {
                    Console.WriteLine("N ITEM PRICE");
                    var line = Console.ReadLine();
                    var nItemPrice = line.Split(" ");
                    if (nItemPrice.Count() == 3)
                    {

                        int n = 1;
                        int.TryParse(nItemPrice[0], out n);

                        var itemId = nItemPrice[1];

                        int itemPrice = 1;
                        int.TryParse(nItemPrice[2], out itemPrice);

                        this._promotionService.CreatePromotion(OfferType.BUY_N_ITEMS_FOR_FIXED,
                                                              new List<OfferItem> { new OfferItem(itemId, n) }, itemPrice);

                    }
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    Console.WriteLine("ITEM ITEM ITEM ... PRICE");
                    var linee = Console.ReadLine();
                    var itemItemPrice = linee.Split(" ");
                    if (itemItemPrice.Count() >= 3)
                    {

                        List<OfferItem> items = new List<OfferItem>();
                        foreach (var i in itemItemPrice)
                        {
                            if (i != itemItemPrice[itemItemPrice.Count() - 1])
                            {
                                items.Add(new OfferItem(i, 1));
                            }
                        }

                        int itemPrice = 1;
                        int.TryParse(itemItemPrice[itemItemPrice.Count() - 1], out itemPrice);

                        this._promotionService.CreatePromotion(OfferType.BUY_COMBINED_ITEMS_FOR_FIXED,
                                                              items, itemPrice);

                    }

                }
            }

            return false;
        }

        private bool Checkout()
        {
            Console.WriteLine("--------- Checkout -----------");
            Console.WriteLine("ID Qty Amount OfferAmount");
            var total = this._orderService.Checkout();
            foreach (var orderItem in this._orderService.GetCart())
            {
                Console.WriteLine(orderItem.Id + " " + orderItem.Quantity + "   " +
                                  orderItem.Amount + "     " + orderItem.OfferAmount);
            }
            Console.WriteLine("Total ---------------  " + total);
            return false;
        }

        private bool MainMenu()
        {

            Console.WriteLine("--------- Operator Commands -----------");
            Console.WriteLine("'a' for AddToCart");
            Console.WriteLine("'c' for CheckOut");
            Console.WriteLine("'n' for New SKU");
            Console.WriteLine("'p' for New Promotion");
            Console.WriteLine("'r' for Clear Cart");
            Console.WriteLine("'q' for Quit");

            var consoleKey = Console.ReadKey();
            if (consoleKey.Key == ConsoleKey.Q)
            {
                return true;
            }
            else if (consoleKey.Key == ConsoleKey.A)
            {
                return AddToCart();
            }
            else if (consoleKey.Key == ConsoleKey.C)
            {
                Checkout();
                Console.ReadKey(true);
                return false;
            }
            else if (consoleKey.Key == ConsoleKey.R)
            {
                return ClearCart();
            }
            else if (consoleKey.Key == ConsoleKey.N)
            {
                return NewSKU();
            }
            else if (consoleKey.Key == ConsoleKey.P)
            {
                return NewPromotion();
            }

            return false;
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

                bool quit = false;
                while (!quit) {

                    PrintStore();

                    Console.WriteLine("Items in your Cart  :");
                    foreach (var orderItem in this._orderService.GetCart())
                    {
                        Console.WriteLine(orderItem.Id + " " + orderItem.Quantity);
                    }

                    quit = MainMenu();
                }
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
