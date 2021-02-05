using System;
namespace promotionengine.Store
{
    public interface IStoreService
    {
        public bool CreateStore (string name);
        public bool RunStore (string name);
    }
}
