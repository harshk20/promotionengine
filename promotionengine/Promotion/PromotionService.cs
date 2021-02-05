using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace promotionengine.Promotion
{
    public class PromotionService : IPromotionService
    {
        private readonly ICollection<Promotion> _promotions;

        public PromotionService()
        {
            this._promotions = new Collection<Promotion>();
        }

        

        public ICollection<Promotion> Promotions { get { return this._promotions; } }

        public bool CreatePromotion(OfferType offerType, List<OfferItem> offerItems, int fixedPrice)
        {
            try
            {
                Promotion promotion = new Promotion(offerType);
                foreach (var offerItem in offerItems)
                    promotion.AddOfferItem(offerItem.Id, offerItem.MinQuantity);
                promotion.FixedPrice = fixedPrice;
                promotion.EnablePromotion();

                this._promotions.Add(promotion);
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }

        public List<Promotion> GetPromotionById(string id)
        {
            try
            {
                if (!this._promotions.Any(p => p.OfferItems.Any(oi => oi.Id.Equals(id))))
                    return null;

                return this._promotions.Where(p => p.OfferItems.Any(oi => oi.Id.Equals(id))).ToList();
            }
            catch(Exception ex)
            {
                _ = ex;
                return null;
            }
        }

        public bool IsPromotionApplicable(string id, int qty)
        {
            try
            {
                var promotions = GetPromotionById(id);

                if (promotions == null)
                    return false;

                bool isApplicable = false;
                foreach (var promotion in promotions)
                {
                    foreach (var offerItem in promotion.OfferItems)
                    {
                        if (offerItem.Id.Equals(id) && offerItem.MinQuantity <= qty)
                        {
                            isApplicable = true;
                            return isApplicable;
                        }
                    }
                }

                return isApplicable;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }
    }
}
