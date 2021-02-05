using System;
using Xunit;
using promotionengine.Inventory;

namespace promotionengine.tests.Inventory
{
    public class InventoryTests
    {
        private readonly IInventoryService _inventoryService;
        public InventoryTests()
        {
            this._inventoryService = new InventoryService();
        }

        [Theory]
        [InlineData("A", 20, "$")]
        [InlineData("B", 30, "$")]
        [InlineData("C", 40, "$")]
        [InlineData("D", 40, "€")]
        public void SKUCreation_Test (string id, int price, string currency)
        {
            Assert.True(this._inventoryService.CreateSKU(id, price, currency), "SKU " + id + " created");
        }

    }
}
