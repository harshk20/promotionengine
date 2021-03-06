﻿using System;
using System.Collections.Generic;

namespace promotionengine.Promotion
{
    public interface IPromotionService
    {
        public bool IsPromotionApplicable(string id, int qty);
        public bool CreatePromotion(OfferType offerType, List<OfferItem> offerItems, int fixedPrice);
        public void GenerateDefaultPromotion();
        public List<Promotion> GetPromotionById(string id);
        public ICollection<Promotion> GetPromotionOffers();
    }
}
