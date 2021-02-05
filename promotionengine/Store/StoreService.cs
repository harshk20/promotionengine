using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace promotionengine.Store
{
    public class StoreService : IStoreService
    {
        private readonly ICollection<Store> _stores;    // member to hold many stores
        private Store _currentStore;                    // member for operating on current store

        public StoreService()
        {
            this._stores = new Collection<Store>();
        }

        // For creating a store
        public bool CreateStore(string name)
        {
            try
            {
                if (_stores.Any(s => s.Name == name))
                    return false;

                _stores.Add(new Store(name));
                return true;
            } catch(Exception ex)
            {
                _ = ex;
                return false;
            }
        }

        // For running a particular store
        public bool RunStore(string name)
        {
            try
            {
                var store = _stores.First(s => s.Name == name);

                if (store == null)
                    return false;

                this._currentStore = store;
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }
    }
}
