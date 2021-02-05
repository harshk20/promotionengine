using System;
namespace promotionengine.Promotion
{
    /**
     * Class to manage OfferItems
     */
    public class OfferItem
    {
        public string Id { get; set; }         // Member to hold item name
        public int MinQuantity { get; set; }               // Member to hold the minimum quantity of stock item
        
        // Constructor
        public OfferItem(string id, int minQuantity)
        {
            this.Id = id;
            this.MinQuantity = minQuantity;
        }
        
    }
}
