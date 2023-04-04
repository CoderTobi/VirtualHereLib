# VirtualHereLib
This is a rather simple library to control the VirtualHere client on Windows using C#.

## Features
- start the application
- start the application minimized
- get all available devices 
- use a device
- stop using a device
- close the application
- 
## New Features
The library currently only features things that I personally needed. If you need a Feature that is currently not available in this library, feel free to suggest it and I might look into it.

## Example
The library is pretty simple to use as you can see:
```
using System;
using VirtualHereLib;

namespace VirtualHereTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a client object representing the VirtualHere application and pass on the path to the application.
            VirtualHereClient client = new VirtualHereClient("E:\\AppsNoInstall\\VirtualHere");

            // Start the application. Pass ‘true’ as a parameter if you want to start it minimized
            client.start(true);

            // Give the application some time to start.
            Task.Delay(2000).Wait();

            // Tell the library to connect to the application
            client.connectToApplication();

            // Get all devices available to the client
            VirtualHereDevice[] devices = client.getAvailableDevices();

            for(int i = 0; i < devices.Length; i++)
            {
                VirtualHereDevice device = devices[i];

                // Look for the device you want to use
                if (device.deviceProduct.Contains("Epson Perfection V33"))
                {
                    // Tell the client to connect the device
                    client.useDevice(device);

                    // Do something while the device is connected
                    Task.Delay(4000).Wait();

                    // Tell the client to disconnect the device again
                    client.stopUsingDevice(devices[0]);                    
                }
            }

            Task.Delay(1500).Wait();

            // Close the application
            // Note: connectToApplication() must be called bevor stop() can be called
            client.stop();
        }
    }
}
```

## Note
I am still very new to Git Hub and only do this in my freetime. Also, I am not affiliated with the makers of VirtualHere.
