using System;
using VirtualHereLib;

namespace VirtualHereTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VirtualHereClient client = new VirtualHereClient("E:\\AppsNoInstall\\VirtualHere");
            client.start(true);

            Task.Delay(2000).Wait();

            client.connectToApplication();
            VirtualHereDevice[] devices = client.getAvailableDevices();

            client.useDevice(devices[0]);

            Task.Delay(4000).Wait();

            client.stopUsingDevice(devices[0]);

            Task.Delay(4000).Wait();

            client.stop();
        }
    }
}