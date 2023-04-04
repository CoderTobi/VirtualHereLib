using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualHereLib
{
    public class VirtualHereDevice
    {
        public string hub { get; }
        public int address { get; }
        public string deviceVendor { get; }
        public string deviceVendorID { get; }
        public string deviceProduct { get; }
        public string deviceProductID { get; }
        public string usedBy { get; }

    public VirtualHereDevice(string pHub, int pAddress, string pDeviceProduct, string pDeviceProductID, string pDeviceVendor, string pDeviceVendorID, string pUsedBy) 
        { 
            hub = pHub;
            address = pAddress;
            deviceVendor = pDeviceVendor;
            deviceVendorID = pDeviceVendorID;
            deviceProduct = pDeviceProduct;
            deviceProductID = pDeviceProductID;
            usedBy = pUsedBy;
        }

    }
}
