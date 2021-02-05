using System;
using Xunit;
using promotionengine.Order;
using promotionengine.Inventory;

namespace promotionengine.tests.Order
{
    public class OrderTests
    {
        private readonly IInventoryService _inventoryService;
        private readonly IOrderService _orderService;
        public OrderTests()
        {
            this._inventoryService = new InventoryService();
            this._orderService = new OrderService(this._inventoryService);
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
            Assert.True(this._orderService.AddToCart(id, qty));
        }
    }
}
