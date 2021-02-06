using System;
using promotionengine.Inventory;
using promotionengine.Order;
using promotionengine.Promotion;
using promotionengine.Store;
using Xunit;

namespace promotionengine.tests.Store
{
    public class StoreTests
    {
        private readonly IInventoryService _inventoryService;
        private readonly IPromotionService _promotionService;
        private readonly IOrderService _orderService;
        readonly private IStoreService _storeService;

        public StoreTests()
        {
            this._inventoryService = new InventoryService();
            this._promotionService = new PromotionService();
            this._orderService = new OrderService(this._inventoryService, this._promotionService);
            this._storeService = new StoreService(this._inventoryService, this._promotionService, this._orderService);
        }

        
        [Theory]
        [InlineData("Alphabets")]
        public void OpenStore_Test(string name)
        {
            Assert.True(_storeService.OpenStore(name), "System opened up a store with name: " + name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void OpenStore_EmptyName_Test(string name)
        {
            Assert.False(_storeService.OpenStore(name));
        }

    }
}
