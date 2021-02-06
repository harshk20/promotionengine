using System;
using Xunit;
using promotionengine.Order;
using promotionengine.Inventory;
using promotionengine.Promotion;
using System.Collections.Generic;

namespace promotionengine.tests.Order
{
    public class OrderTests
    {
        private readonly IInventoryService _inventoryService;
        private readonly IOrderService _orderService;
        private readonly IPromotionService _promotionService;
        public OrderTests()
        {
            this._inventoryService = new InventoryService();
            this._promotionService = new PromotionService();
            this._orderService = new OrderService(this._inventoryService,
                                                  this._promotionService);

        }

        [Fact]
        public void CheckIfCartIsEmpty_Test()
        {
            Assert.True(this._orderService.IsCartEmpty());
        }

        [Theory]
        [InlineData("A", 4)]
        public void AddItemToCart_Test(string id, int qty)
        {
            this._inventoryService.CreateSKU(id, 10, "");
            Assert.True(this._orderService.AddToCart(id, qty));
        }

        [Theory]
        [InlineData("A", -1)]
        public void AddItemToCart_Negative_Test(string id, int qty)
        {
            this._inventoryService.CreateSKU(id, 10, "");
            Assert.False(this._orderService.AddToCart(id, qty));
        }

        [Fact]
        public void CheckIfCartIsEmptyAfterAddToCart_Test()
        {
            AddItemToCart_Test("A", 2);
            Assert.False(this._orderService.IsCartEmpty());
        }

        [Theory]
        [InlineData("A")]
        public void FindCartItemById_Test(string id)
        {
            Assert.Null(this._orderService.FindById(id));
            AddItemToCart_Test(id, 1);
            Assert.NotNull(this._orderService.FindById(id));
        }

        [Theory]
        // "ID",CartQty,UnitPrice,PromotionQty,PromotionFixedPrice,ExpectedTotal
        [InlineData("A", 4, 10, 3, 25, 35)]
        [InlineData("B", 1, 10, 3, 25, 10)]
        public void CheckoutTotal_Test(string id, int qty, int price, int minPromoQty, int fixedPrice, int total)
        {
            this._inventoryService.CreateSKU(id, price, "$");
            this._promotionService.CreatePromotion(OfferType.BUY_N_ITEMS_FOR_FIXED,
                                                new List<OfferItem> { new OfferItem(id, minPromoQty) },
                                                fixedPrice);
            AddItemToCart_Test(id, qty);
            Assert.True(this._orderService.Checkout() == total);

        }

        [Theory]
        // "ID",CartQty,UnitPrice,promtionItem,PromotionFixedPrice,ExpectedTotal
        [InlineData("A", 4, 10, "B", 15, 60)]
        [InlineData("B", 1, 10, "C", 15, 15)]
        public void CheckoutTotal_Test2(string id, int qty, int price, string promotionItem, int fixedPrice, int total)
        {
            this._inventoryService.CreateSKU(id, price, "$");
            this._inventoryService.CreateSKU(promotionItem, price, "$");

            this._promotionService.CreatePromotion(OfferType.BUY_COMBINED_ITEMS_FOR_FIXED,
                                                new List<OfferItem> { new OfferItem(id, 1),
                                                                      new OfferItem(promotionItem, 1)},
                                                fixedPrice);
            AddItemToCart_Test(id, qty);
            AddItemToCart_Test(promotionItem, qty);
            Assert.True(this._orderService.Checkout() == total);

        }

        [Theory]
        // Item1,UnitPrice1, CartQty1, Item2, UnitPrice2, CartQty2, promtionQty, PromotionFixedPrice, ExpectedTotal
        [InlineData("A", 10, 4, "B", 20, 2, 1, 25, 70)]
        public void CheckoutTotal_Test3(string id1, int price1, int qty1, string id2, int price2,  int qty2, int minPromoQty, int fixedPrice, int total)
        {
            this._inventoryService.CreateSKU(id1, price1, "$");
            this._inventoryService.CreateSKU(id2, price2, "$");

            this._promotionService.CreatePromotion(OfferType.BUY_COMBINED_ITEMS_FOR_FIXED,
                                                new List<OfferItem> { new OfferItem(id1, minPromoQty),
                                                                      new OfferItem(id2, minPromoQty)},
                                                fixedPrice);
            AddItemToCart_Test(id1, qty1);
            AddItemToCart_Test(id2, qty2);
            Assert.True(this._orderService.Checkout() == total);

        }
    }
}