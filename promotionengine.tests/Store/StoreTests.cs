using System;
using promotionengine.Store;
using Xunit;

namespace promotionengine.tests.Store
{
    public class StoreTests
    {
        readonly private IStoreService _storeService;

        public StoreTests()
        {
            this._storeService = new StoreService();
        }

        [Theory]
        [InlineData("Alphabets")]
        [InlineData("Numbers")]
        public void StoreCreation_Test(string name)
        {
            Assert.True(_storeService.CreateStore(name), "Store Created with name : " + name);
        }

        [Theory]
        [InlineData("Alphabets")]
        public void StoreDuplication_Test(string name)
        {
            StoreCreation_Test(name);
            Assert.False(_storeService.CreateStore(name), "Store duplicate with name : " + name);

        }

        [Theory]
        [InlineData("Alphabets")]
        public void RunStoreBeforeCreation_Test(string name)
        {
            Assert.False(_storeService.RunStore(name), "System didn't find store with name: " + name);
        }

        [Theory]
        [InlineData("Alphabets")]
        public void RunStoreAfterCreation_Test(string name)
        {
            StoreCreation_Test(name);
            Assert.True(_storeService.RunStore(name), "System is running " + name + "store right now");
        }

    }
}
