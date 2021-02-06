using System;
using Xunit;
using promotionengine.Order;
using promotionengine.Inventory;
using promotionengine.Promotion;

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


    }
}
