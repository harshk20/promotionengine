using System;
using Xunit;
using promotionengine.Promotion;

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
        [InlineData("A", 4)]
        public void CheckIfPromotionIsApplicable_Test(string id, int qty)
        {
            Assert.True(this._promotionService.IsPromotionApplicable(id, qty));
        }
    }
}
