using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualHereLib.Exceptions
{
    public class NotConnectedToVirtualHereException : Exception
    {
        static readonly string message = "Connection with the VirtualHere Client has not been established yet. " +
            "Did you forget to call the method connectToApplication()?";
        public NotConnectedToVirtualHereException() : base(message)
        {

        }
    }
}
