using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading;

namespace MNApp.Lib
{
    public class EMailExtract
    {
        public static bool escPressed = false;
        private string currentDir;

        public string CurrentDir
        {
            get
            {
                if (String.IsNullOrEmpty(currentDir))   CurrentDir = Directory.GetCurrentDirectory();
                
                return currentDir;
            }
            set { currentDir = value; }
        }


        private string inputDir;

        public string InputDir
        {
            get
            {
                if (String.IsNullOrEmpty(inputDir))
                {
                    InputDir = $"{CurrentDir}/Input";
                }
                return inputDir;
            }
            set { inputDir = value; }
        }

        public bool InputDirExists
        {
            get
            {                
                return Directory.Exists(InputDir);
            }
        }

        public string[] Files
        {
            get
            {                
                return Directory.GetFiles(InputDir, "*.txt", SearchOption.TopDirectoryOnly);
            }
        }
        
        public List<FileInfo> FileDetails
        {
            get
            {
                List<FileInfo> fileInfos = new List<FileInfo>();
                foreach(string file in Files)
                {
                    fileInfos.Add(new FileInfo(file));
                }
                return fileInfos;
            }
        }

        public static bool PingHost(string nameOrAddress,bool IsWriteLog = true)
        {
            if(IsWriteLog)Console.WriteLine($"{nameOrAddress} Ping  start");
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
            if (IsWriteLog) Console.WriteLine($"{nameOrAddress} Ping  result {pingable}");
            if (IsWriteLog) Console.WriteLine($"{nameOrAddress} Ping  end");
            return pingable;
        }

        public static long NetConnectionCheckAndWait()
        {
            long n = 1;
            bool r = false;
            while (!EMailExtract.escPressed)
            {
                r = PingHost("google.com",false);
                if (r) return n; 
                Console.WriteLine($"NetConnectionCheckAndWait => {n++}");
                Thread.Sleep(2000);
            }
            Console.WriteLine("Escape key pressed");
            return n;
        }

    }
}
