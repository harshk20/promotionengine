using System;
namespace promotionengine.Store
{
    /**
     * Class to store the store's properties
     */
    public class Store
    {
        private readonly string _name;          // Member to hold store name
        public Store(string name)
        {
            this._name = name;
        }

        public string Name { get { return this._name; } }
    }
}
