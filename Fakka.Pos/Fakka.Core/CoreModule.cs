using System.ComponentModel;
using System.Threading;
using Fakka.Core.Constants;
using Fakka.Core.Interfaces;
using Fakka.Core.Managers;
using Fakka.Core.Modules;
using Fakka.Core.Providers;
using Fakka.Core.Utilities;
using Prism.Ioc;

namespace Fakka.Core
{
    public class CoreModule : BaseModule
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register Services
            containerRegistry.RegisterSingleton<INetwork, HttpProvider>();
            containerRegistry.RegisterSingleton<IStorage, SqliteStorageProvider>();
            containerRegistry.RegisterSingleton<IView, ViewProvider>();

            containerRegistry.RegisterSingleton<IDataService, DataService>();
            containerRegistry.RegisterSingleton<IStorageManager, StorageManager>();


            containerRegistry.RegisterSingleton<ICameraHandler, CameraHandler>();
            containerRegistry.RegisterSingleton<IBluetoothHandler, BluetoothHandler>();

            containerRegistry.RegisterSingleton<IDataContext, BaseDataContextProvider>();
            // Collection of all Data Services



        }
    }
}
