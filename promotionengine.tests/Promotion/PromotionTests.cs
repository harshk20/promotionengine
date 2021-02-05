using System;
using Xunit;
using promotionengine.Promotion;
using System.Collections.Generic;

namespace promotionengine.tests.Promotion
{
    public class PromotionTests
    {
        private readonly IPromotionService _promotionService;
        public PromotionTests()
        {
            this._promotionService = new PromotionService();
        }

        [Theory]
        [InlineData(OfferType.BUY_N_ITEMS_FOR_FIXED, 4, "A", 10)]
        public void CreatePromotionNItemsForFixed(OfferType offerType, int n, string itemId, int fixedPrice)
        {
            Assert.True(this._promotionService.CreatePromotion(offerType, new List<OfferItem> { new OfferItem(itemId, n) }, fixedPrice));
        }

        [Theory]
        [InlineData(OfferType.BUY_COMBINED_ITEMS_FOR_FIXED, "A", "B", 10)]
        public void CreatePromotionCombinedItemsForFixed(OfferType offerType, string item1, string item2, int fixedPrice)
        {
            Assert.True(this._promotionService.CreatePromotion(offerType, new List<OfferItem> { new OfferItem(item1, 1), new OfferItem(item2, 1) }, fixedPrice));
        }

        [Theory]
        [InlineData("A", 4)]
        public void CheckIfPromotionIsApplicableBeforeCreate_Test(string id, int qty)
        {
            Assert.False(this._promotionService.IsPromotionApplicable(id, qty));
        }

        [Theory]
        [InlineData("A", 4)]
        public void CheckIfPromotionIsApplicableAfterCreate_Test(string id, int qty)
        {
            CreatePromotionNItemsForFixed(OfferType.BUY_N_ITEMS_FOR_FIXED, qty, id, 10);
            Assert.True(this._promotionService.IsPromotionApplicable(id, qty));
        }

        [Theory]
        [InlineData("A", 1)]
        [InlineData("B", 1)]
        public void CheckIfPromotionIsApplicableAfterCreate_Test2(string id, int qty)
        {
            CreatePromotionCombinedItemsForFixed(OfferType.BUY_COMBINED_ITEMS_FOR_FIXED, "A", "B", 10);
            Assert.True(this._promotionService.IsPromotionApplicable(id, qty));
        }

        [Theory]
        [InlineData("A")]
        public void GetPromotionById_Test(string id)
        {
            CreatePromotionNItemsForFixed(OfferType.BUY_N_ITEMS_FOR_FIXED, 10, id, 10);
            Assert.NotNull(this._promotionService.GetPromotionById(id));

        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        public void GetPromotionById_Test2(string id)
        {
            CreatePromotionCombinedItemsForFixed(OfferType.BUY_COMBINED_ITEMS_FOR_FIXED, "A", "B", 10);
            Assert.NotNull(this._promotionService.GetPromotionById(id));

        }

    }
}
