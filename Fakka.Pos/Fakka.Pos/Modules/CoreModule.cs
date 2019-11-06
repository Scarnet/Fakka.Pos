using System;
using System.Collections.Generic;
using System.Text;
using Fakka.Core.Constants;
using Fakka.Core.Interfaces;
using Fakka.Core.Managers;
using Fakka.Core.Providers;
using Fakka.Core.Utilities;
using Fakka.Pos.Managers;
using Fakka.Pos.Providers;
using Prism.Ioc;

namespace Fakka.Pos.Modules
{
    public class CoreModule : BaseModule
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.Register<ISessionManager, PresistentSessionManager>();
            containerRegistry.RegisterSingleton<INetwork, HttpProvider>();
            containerRegistry.RegisterSingleton<IStorage, SqliteStorageProvider>();
            containerRegistry.RegisterSingleton<IView, ViewProvider>();

            containerRegistry.RegisterSingleton<IDataService, DataService>();
            containerRegistry.RegisterSingleton<IStorageManager, StorageManager>();


            containerRegistry.RegisterSingleton<IBluetoothHandler, BluetoothHandler>();

            containerRegistry.RegisterSingleton<IDataContext, DataContextProvider>();


        }
    }
}
