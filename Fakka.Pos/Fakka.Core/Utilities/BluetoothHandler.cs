using Fakka.Core.Interfaces;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace Fakka.Core.Utilities
{
  
    public class BluetoothHandler : IBluetoothHandler
    {
        public BluetoothHandler()
        {
            Ble = CrossBluetoothLE.Current;
            BleAdapter = CrossBluetoothLE.Current.Adapter;
        }


        public IBluetoothLE Ble { get; private set; }
        public IAdapter BleAdapter { get; private set; }
    }

    
}