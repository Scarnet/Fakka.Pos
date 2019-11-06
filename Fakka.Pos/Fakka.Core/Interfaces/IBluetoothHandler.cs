
using System;
using Plugin.BLE.Abstractions.Contracts;

namespace Fakka.Core.Interfaces
{
    public interface IBluetoothHandler
    {
        IBluetoothLE Ble { get; }
        IAdapter BleAdapter { get; }
    }
    
}
