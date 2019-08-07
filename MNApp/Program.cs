using System;
using MNApp.Lib;

namespace MNApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EMailExtract EME = new EMailExtract();

            if (!EME.InputDirExists)
            {
                Console.WriteLine($"{EME.InputDir} does not exist.");
                Console.ReadKey();
                return;
            }

            foreach(var file in EME.FileDetails)
            {
                Console.WriteLine($"{file.Name} {file.LastWriteTime}");
            }

            Console.ReadKey();
        }
    }
}
