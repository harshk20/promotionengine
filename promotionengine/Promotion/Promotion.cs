using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace promotionengine.Promotion
{
    public class Promotion
    {
        private ICollection<OfferItem> _offerItems;
        private OfferType _offerType;

        public Promotion(OfferType offerType)
        {
            this._offerType = offerType;
            _offerItems = new Collection<OfferItem>();
        }

        public void AddOfferItem(string id, int qty)
        {
            if (!this._offerItems.Any(oi => oi.Id.Equals(id)))
                this._offerItems.Add(new OfferItem(id, qty));
        }
        public void EnablePromotion()
        {
            IsActive = true;
        }
        public void DisablePromotion()
        {
            IsActive = false;
        }
        public bool IsActive { get; set; }
        public int FixedPrice { get; set; }
        public OfferType OfferType { get; set; }
        public ICollection<OfferItem> OfferItems { get { return this._offerItems; } }
    }
}
