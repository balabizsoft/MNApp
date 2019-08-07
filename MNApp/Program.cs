using System;
using MNApp.Lib;
using MNApp.DAL;
using Microsoft.EntityFrameworkCore;
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
            MNAppDBContext db = new MNAppDBContext();
            db.Database.Migrate();

            foreach (var file in EME.FileDetails)
            {
                var d = db.FileDetails.FirstOrDefaultAsync(x => x.Name == file.Name).Result;
                if (d == null)
                {
                    d = new FileDetail() { Name = file.Name, LastUpdate = file.LastWriteTimeUtc };
                    db.FileDetails.Add(d);
                }
                Console.WriteLine($"{file.Name} {file.LastWriteTime}");
            }
            db.SaveChanges();
            Console.ReadKey();
        }
    }
}
