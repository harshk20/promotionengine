using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using promotionengine.Inventory;
using promotionengine.Promotion;

namespace promotionengine.Order
{
    public class OrderService : IOrderService
    {
        private readonly ICollection<OrderItem> _cart;
        private readonly IInventoryService _inventoryService;
        private readonly IPromotionService _promotionService;

        public OrderService (IInventoryService inventoryService,
                             IPromotionService promotionService)
        {
            this._inventoryService = inventoryService;
            this._promotionService = promotionService;
            this._cart = new Collection<OrderItem>();
        }

        /**
         * To add items by id in cart, if item is already added, this will add more quantity
         */
        public bool AddToCart(string id, int qty = 1)
        {
            try
            {
                // First we have to check if the Stock item actually exists with this id
                var sku = this._inventoryService.GetSKUById(id);
                if (sku == null || qty < 0)
                    return false;

                OrderItem orderItem = FindById(id);
                if (orderItem == null)
                {
                    orderItem = new OrderItem(id, qty);
                    orderItem.Price = sku.Price;
                    orderItem.Currency = sku.Currency;

                    this._cart.Add(orderItem);
                }
                else
                {
                    orderItem.AddQuantity(qty);
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
            throw new NotImplementedException();
        }

        /**
         * This will return Checkout Amount
         */
        public int Checkout()
        {
            ClearOldOfferAndTotals();
            ComputeAmountWithPromotion();
            CalculateTotals();

            return CartOfferTotal;
        }

        // To check if the cart is empty or not
        public bool IsCartEmpty()
        {
            return this.Cart.Count == 0;
        }

        // To find order item by Id
        public OrderItem FindById(string id)
        {
            try
            {
                if (!this._cart.Any(oi => oi.Id.Equals(id)))
                    return null;

                return this._cart.Where(oi => oi.Id.Equals(id)).Single();
            }
            catch (Exception ex)
            {
                _ = ex;
                return null;
            }
        }

        private void ComputeAmountWithPromotion()
        {
            try
            {
                foreach (var orderItem in Cart)
                {
                    // Original total amount
                    orderItem.Amount = orderItem.Price * orderItem.Quantity;

                    // See if offer needs to be applied
                    if (!orderItem.IsOfferApplied && this._promotionService.IsPromotionApplicable(orderItem.Id, orderItem.Quantity))
                    {
                        var promotionList = this._promotionService.GetPromotionById(orderItem.Id);
                        // Get the first promotion and apply that
                        var promotion = promotionList.First();

                        // Case buy n items for fixed price
                        if(promotion.OfferItems.Count == 1 && promotion.OfferType == OfferType.BUY_N_ITEMS_FOR_FIXED)
                        {
                            var OfferItemPrice = (orderItem.Quantity / promotion.OfferItems.First().MinQuantity) * promotion.FixedPrice;
                            var remainingItemsPrice = (orderItem.Quantity % promotion.OfferItems.First().MinQuantity) * orderItem.Price;

                            orderItem.OfferAmount = OfferItemPrice + remainingItemsPrice;
                            orderItem.IsOfferApplied = true;

                        // Case buy item1 and item2 for fixed price
                        } else if(promotion.OfferItems.Count > 1 && promotion.OfferType == OfferType.BUY_COMBINED_ITEMS_FOR_FIXED)
                        {
                            bool isPromotionApplicable = true;
                            int offerMinQty = int.MaxValue;
                            foreach (var offerItem in promotion.OfferItems)
                            {
                                var item = FindById(offerItem.Id);
                                // We need to check all other items to see if this promotion is still applicable
                                if (item == null || item.IsOfferApplied || offerItem.MinQuantity > item.Quantity)
                                    isPromotionApplicable = false;

                                var offerQty = item.Quantity / offerItem.MinQuantity;

                                if (offerMinQty > offerQty)
                                    offerMinQty = offerQty;

                            }

                            if (isPromotionApplicable)
                            {
                                OrderItem lastItem = orderItem;
                                // Mark all Items as OfferApplied
                                foreach (var offerItem in promotion.OfferItems)
                                {
                                    lastItem = FindById(offerItem.Id);

                                    lastItem.OfferAmount = (lastItem.Quantity - offerMinQty) * lastItem.Price;
                                    lastItem.IsOfferApplied = true;
                                }

                                lastItem.OfferAmount += offerMinQty * promotion.FixedPrice;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
            }
        }

        private void CalculateTotals()
        {
            try
            {
                if (IsCartEmpty())
                    return;

                foreach (var orderItem in Cart)
                {
                    CartTotal += orderItem.Amount;
                    CartOfferTotal += (orderItem.IsOfferApplied == true)? orderItem.OfferAmount : orderItem.Amount;
                }

            }
            catch (Exception ex)
            {
                _ = ex;
            }
        }

        private void ClearOldOfferAndTotals()
        {
            try
            {
                CartOfferTotal = 0;
                CartTotal = 0;
                if (IsCartEmpty())
                    return;

                foreach (var orderItem in Cart)
                {
                    orderItem.Amount = 0 ;
                    orderItem.OfferAmount = 0;
                    orderItem.IsOfferApplied = false;
                }
                
            }
            catch (Exception ex)
            {
                _ = ex;
            }
        }
        

        public ICollection<OrderItem> GetCart()
        {
            return this.Cart;
        }

        public void EmptyCart()
        {
            this._cart.Clear();
        }

        public ICollection<OrderItem> Cart { get { return this._cart; } }
        public int CartTotal { get; set; }
        public int CartOfferTotal { get; set; }

    }
}
