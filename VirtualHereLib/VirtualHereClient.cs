using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO.Pipes;
using VirtualHereLib.Exceptions;

namespace VirtualHereLib
{
    public class VirtualHereClient
    {
        string folderPath;
        string execName = "vhui64.exe";

        Process vhProcess = null;
        NamedPipeClientStream pipe;

        public VirtualHereClient(string pPathToExecFolder) 
        {
            folderPath = pPathToExecFolder;
        }   

        public VirtualHereClient(string pPathToExecFolder, string pExecName)
        {
            folderPath = pPathToExecFolder;
            execName = pExecName;
        }

        ~VirtualHereClient()
        {
            if (pipe != null)
            {
                if (pipe.IsConnected)
                {
                    pipe.Close();
                }
            }
        }

        public void start(bool pMinimized)
        {
            vhProcess = new Process();
            vhProcess.StartInfo.FileName = folderPath + "/" + execName;
            if (pMinimized)
            {
                vhProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                vhProcess.StartInfo.Arguments = "-g";
            }
            vhProcess.Start();
        }

        public void connectToApplication() 
        {
            pipe = openPipe();
        }

        public VirtualHereDevice[] getAvailableDevices()
        {
            List<VirtualHereDevice> devices = new List<VirtualHereDevice>();

            string[]output = RunCmdAndGetOutput("list");

            if (output.Length < 6)
            {
                return null;
            }

            int counter = 4;

            while(counter < output.Length)
            {
                string tmp = output[counter];
                counter++;
                if(!tmp.StartsWith("   -->"))
                {
                    counter++;
                    continue;
                }

                tmp = tmp.Substring(6);
                string fullAddress = strBetweenChars('(', ')', tmp);
                string hubName = fullAddress.Split(".")[0];
                int deviceAddress = int.Parse(fullAddress.Split(".")[1]);

                //Get additional stuff
                string[] outputDeviceInfo = RunCmdAndGetOutput("device info," + fullAddress);
                string deviceVendor = outputDeviceInfo[1].Split(": ")[1];
                string deviceVendorID = outputDeviceInfo[2].Split(": ")[1];
                string deviceProduct = outputDeviceInfo[3].Split(": ")[1];
                string deviceProductID = outputDeviceInfo[4].Split(": ")[1];
                string deviceUser = outputDeviceInfo[5].Split(": ")[1];
                if (deviceUser == "NO ONE")
                {
                    deviceUser = null;
                }

                //create Device obj
                VirtualHereDevice device = new VirtualHereDevice(hubName, deviceAddress, deviceProduct, deviceProductID, deviceVendor, deviceVendorID, deviceUser);
                devices.Add(device);
            }

            return devices.ToArray();
        }

        public void useDevice(VirtualHereDevice pDevice)
        {
            string address = pDevice.hub + "." + pDevice.address;
            RunCmd("use," + address);
        }

        public void stopUsingDevice(VirtualHereDevice pDevice)
        {
            string address = pDevice.hub + "." + pDevice.address;
            RunCmd("stop using," + address);
        }

        public void stop()
        {
            //vhProcess.CloseMainWindow();
            //vhProcess.Kill();
            RunCmd("exit");
        }

        private string[] RunCmdAndGetOutput(string pCmd)
        {            
            List<string> result = new List<string>();

            RunCmd(pCmd);

            StreamReader sr = new StreamReader(pipe);

            List<char> buffer = new List<char>();

            while (sr.Peek() >= 0)
            {
                char temp = (char)sr.Read();
                buffer.Add(temp);
            }

            string line = "";
            for(int i  = 0; i < buffer.Count; i++)
            {
                if (buffer[i] == '\n')
                {
                    result.Add(line);
                    //Console.WriteLine("Received from server: {0}", line);
                    line = "";
                }
                else
                {
                    line += buffer[i];
                }
            }

            return result.ToArray();
        }

        private void RunCmd(string pCmd)
        {
            if(pipe == null)
            {
                throw new VirtualHereLib.Exceptions.NotConnectedToVirtualHereException();
            }
            
            pipe.Flush();

            char[] WriteCMD = pCmd.ToCharArray();

            StreamWriter sw = new StreamWriter(pipe);
            sw.AutoFlush = true;

            sw.Flush();
            sw.Write(WriteCMD);

            pipe.WaitForPipeDrain();

        }

        private NamedPipeClientStream openPipe()
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "vhclient", PipeDirection.InOut);

            try
            {
                pipeClient.Connect(2000);
            }
            catch (System.TimeoutException e)
            {
                throw new VirtualHereNotRunningException(e);
            }

            pipeClient.ReadMode = PipeTransmissionMode.Message;

            return pipeClient;
        }

        private string strBetweenChars(char pStartChar,  char pEndChar, string pString)
        {
            int pFrom = pString.IndexOf(pStartChar) + 1;
            int pTo = pString.IndexOf(pEndChar);

            return pString.Substring(pFrom, pTo - pFrom);
        }
    }
}