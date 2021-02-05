using System;
using Microsoft.Extensions.DependencyInjection;
using promotionengine.Inventory;
using promotionengine.Order;
using promotionengine.Promotion;
using promotionengine.Store;

namespace promotionengine
{
    class Program
    {
        // service provider
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            // Register services for dependency injection
            RegisterServices();

            // Get the store service and start running the store
            var storeService = _serviceProvider.GetRequiredService<IStoreService>();
            storeService.OpenStore("Alphabets");

            // Disponse the services if there is anything to dispose
            DisposeServices();
        }

        /**
         * Register services
         */
        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IStoreService, StoreService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IOrderService, OrderService>();


            _serviceProvider = services.BuildServiceProvider();
        }

        /**
         * Dispose services
         */
        private static void DisposeServices()
        {
            if (_serviceProvider != null && _serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
