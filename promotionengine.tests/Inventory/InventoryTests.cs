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

        [Theory]
        [InlineData ("A")]
        public void SKUGetById_BeforeCreating_Test (string id)
        {
            var sku = this._inventoryService.GetSKUById(id);
            Assert.Null(sku);
        }

        [Theory]
        [InlineData("A", 10, "$")]
        public void SKUGetById_AfterCreating_Test(string id, int price, string currency)
        {
            SKUCreation_Test(id, price, currency);
            var sku = this._inventoryService.GetSKUById(id);
            Assert.NotNull(sku);
            Assert.True(sku.Id.Equals(id));
        }

    }
}
