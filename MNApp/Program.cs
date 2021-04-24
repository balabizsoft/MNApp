using System;
using MNApp.Lib;
using MNApp.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Whois.NET;

namespace MNApp
{
    class Program
    {
        
        public static MNAppDBContext db = new MNAppDBContext();

        private static long recCount;
        public static long RecCount 
        {
            get
            {
                return recCount;
            }
            set 
            {
                recCount = value;
                if (recCount % 1000 == 0) DBSave();
             } 
        }

        public static void DBSave()
        {
            Console.WriteLine("Database changes save start");
            db.SaveChanges();
            Console.WriteLine("Database changes save End");
        }
        static void Main(string[] args)
        {
            Task task = null;
            

            ConsoleKey menuSelect;
            Console.WriteLine("Database Migrate start");
            db.Database.Migrate();
            menuSelect = SelectMenu();
            switch (menuSelect)
            {
                case ConsoleKey.D1: task = Task.Run(()=> DomainNameStore());break;
                case ConsoleKey.D2: task = Task.Run(()=> PingDomain()); break;
                case ConsoleKey.D3: task = Task.Run(() => FetchDate()); break;
                case ConsoleKey.D4: task = Task.Run(() => FetchContactURL()); break;
                case ConsoleKey.D5: task = Task.Run(() => FetchEMail()); break;
                case ConsoleKey.D6: task = Task.Run(() => MediaCheck()); break;
            }
            Task tKeyPresed = Task.Run(() => KeyPresed());
            if(task!=null)
            {
                Console.WriteLine("Task Start");
                task.Wait();
                Console.WriteLine("Task End");
            }
            DBSave();
            Console.WriteLine("done. press escape to exit");
            tKeyPresed.Wait();
        }

        static ConsoleKey SelectMenu()
        {
            List<ConsoleKey> menus = new List<ConsoleKey>() { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.Escape };

            Console.Clear();
            Console.WriteLine("1, Domain Name Store");
            Console.WriteLine("2, Ping Domain");
            Console.WriteLine("3, Fetch Date ");
            Console.WriteLine("4, Fetch Contact URL");
            Console.WriteLine("5, Fetch Email");
            Console.WriteLine("6, Media Check");
            Console.WriteLine("");

            Console.WriteLine("Please Select [1-6] or Press Esc to Exit ");
            ConsoleKey consoleKeyInfo;
            while( !menus.Contains( consoleKeyInfo =Console.ReadKey(true).Key));
            return consoleKeyInfo;
         }

        static void DomainNameStore()
        {
            EMailExtract EME = new EMailExtract();
            if (!EME.InputDirExists)
            {
                Console.WriteLine($"{EME.InputDir} does not exist.");
                return;
            }
            
            foreach (var file in EME.FileDetails)
            {
                FileDetail  f = new FileDetail() { Name = file.Name,DomainDetails=new List<DomainDetail>() };
                db.FileDetails.Add(f);
                RecCount++;
                Console.WriteLine($"File => {file.Name}");
                FileInfo fIn = new FileInfo($"input/{file.Name}");
                foreach (var s in File.ReadAllLines(fIn.FullName))
                {
                    Console.WriteLine(s);
                    f.DomainDetails.Add(new DomainDetail() { Name=s });
                    RecCount++;
                    if (EMailExtract.escPressed) return;
                }
                if (EMailExtract.escPressed) return;
            }
            DBSave();

        }
       
        static void PingDomain()
        {
            foreach(var p in db.DomainDetails.Where(x=> x.IsPing == null))
            {
                bool r = EMailExtract.PingHost(p.Name);
                if (r == true)
                {
                    p.IsPing = true;
                    RecCount++;
                }
                else
                {
                    long n = EMailExtract.NetConnectionCheckAndWait();
                    p.IsPing = (n == 1)? false : EMailExtract.PingHost(p.Name);
                    RecCount++;
                }
                if (EMailExtract.escPressed) return;
            }
        }

        static void FetchDate()
        {
            Whois.Parsers.WhoisParser whoisParser = new Whois.Parsers.WhoisParser();
            foreach (var p in db.DomainDetails.Where(x => x.HasDate == null && x.IsPing==true))
            {
                Console.WriteLine($"Fetch Date => {p.Name}");
                WhoisResponse d = WhoisClient.Query(p.Name);
                var d2 = whoisParser.Parse(d.RespondedServers[d.RespondedServers.Length - 1], d.Raw);

                if (d2.Registered != null && d2.Updated != null && d2.Expiration != null)
                {
                    p.HasDate = true;
                    p.RegisterAt = d2.Registered;
                    p.UpdateAt = d2.Updated;
                    p.ExpiryAt = d2.Expiration;

                    RecCount++;
                }
                else
                {
                    long n = EMailExtract.NetConnectionCheckAndWait();
                    if (n == 1)
                    {
                        p.HasDate = false;
                        RecCount++;
                    }
                    else
                    {
                        d = WhoisClient.Query(p.Name);
                        d2 = whoisParser.Parse(d.RespondedServers[d.RespondedServers.Length - 1], d.Raw);

                        if (d2.Registered != null && d2.Updated != null && d2.Expiration != null)
                        {
                            p.HasDate = true;
                            p.RegisterAt = d2.Registered;
                            p.UpdateAt = d2.Updated;
                            p.ExpiryAt = d2.Expiration;
                            RecCount++;
                        }
                        else
                        {
                            p.HasDate = false;
                            RecCount++;
                        }
                    }
                    
                }
                if (EMailExtract.escPressed) return;
            }
        }

        static void FetchContactURL()
        {
            Console.WriteLine("FetchContactURL coming soon");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static void FetchEMail()
        {
            Console.WriteLine("FetchEMail coming soon");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static void MediaCheck()
        {
            Console.WriteLine("MediaCheck coming soon");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        static void KeyPresed()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
            EMailExtract.escPressed = true;
            Console.WriteLine("Escape key pressed");
        }
    }
}
