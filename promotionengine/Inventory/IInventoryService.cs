using System;
namespace promotionengine.Inventory
{
    public interface IInventoryService
    {
        public bool CreateSKU (string id, int price, string currency);
        public SKU GetSKUById (string id);

    }
}
