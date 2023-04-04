using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VirtualHereLib.Exceptions
{
    public class VirtualHereNotRunningException : Exception
    {
        static readonly string message = "Unable to communicate with the VirtualHere Client. " +
            "Did you forget to start it? " +
            "Starting the client might take some time.";
        public VirtualHereNotRunningException(Exception pBaseException) : base (message,pBaseException)
        {

        }
    }
}
