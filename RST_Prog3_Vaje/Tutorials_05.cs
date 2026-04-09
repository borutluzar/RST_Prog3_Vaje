using System.Net.ServerSentEvents;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public class Tutorials_05
    {
        public enum Exercises
        {
            Exercise_921 = 1
        }

        /// <summary>
        /// Definirajte razred Report in vmesnik IDocument, ki ga Report implementira.
        /// V vmesniku določite nujne (meta)lastnosti, ki jih običajno mora imeti vsak dokument, 
        /// v razredu Report pa jih ustrezno implementirajte. 
        /// Definirajte še vmesnik IWorkflow, ki določa lastnosti in funkcije za pošiljanje poročila po delovnem toku, 
        /// in ga implementirajte razredu Report.
        /// Pri tem dodajte še razred SystemUser, ki bo predstavljal uporabnika delovnega toka v vmesniku IWorkflow. 
        /// Vmesniku IWorkflow definirajte še funkcijo Sign in jo eksplicitno implementirajte v razredu Report, 
        /// obenem pa v razred dodajte tudi funkcijo Sign, ki ni funkcija vmesnika.
        /// </summary>
        public static void Exercise_921()
        {
            ProgramRacun pr = new ProgramRacun();
            pr.Run();
        }
    }

    public sealed class UserPreferences
    {
        private UserPreferences() { }

        private static UserPreferences instance = null;

        public static UserPreferences GetInstance()
        {
            if (instance == null)
            {
                instance = new UserPreferences();
            }
            return instance;
        }

        public int Težavnost { get; private set; } = 10;
        public TipOperacije Operacija { get; private set; } = TipOperacije.Seštevanje;

        public void NastaviOperacijoInTip(TipOperacije operacija, int težavnost)
        {
            Težavnost = težavnost;
            Operacija = operacija;
        }
    }

    public enum TipOperacije
    {
        Seštevanje,
        Množenje
    }

    public sealed class Log
    {
        private Log()
        {
            imeDatoteke = $"Racuni_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log";
        }

        private static Log instance = null;

        public static Log GetInstance()
        {
            if (instance == null)
            {
                instance = new Log();
            }
            return instance;
        }
        string imeDatoteke;

        public void ZapisiVDatoteko(string text)
        {
            StreamWriter sw = new StreamWriter(imeDatoteke, true);
            sw.WriteLine(text);
            sw.Close();
        }
    }


    public class ProgramRacun
    {
        private UserPreferences UserPreferences { get; }
        private Log Log { get; }

        public ProgramRacun()
        {
            Console.WriteLine("Vnesi preference");
            Console.WriteLine("Težavnost: ");
            string tezavnostStr = Console.ReadLine() ?? "10";
            int tezavnost = 10;
            Console.WriteLine("Operacija [1: Seštevanje, 2: Množenje]: ");
            string operacijaStr = Console.ReadLine() ?? "1";
            TipOperacije operacija = TipOperacije.Seštevanje;
            if (int.TryParse(tezavnostStr, out int tezavnost2))
            {
                tezavnost = tezavnost2;
            }
            switch (operacijaStr)
            {
                case "1":
                    operacija = TipOperacije.Seštevanje;
                    break;
                case "2":
                    operacija = TipOperacije.Množenje;
                    break;
            }
            this.UserPreferences = UserPreferences.GetInstance();
            this.UserPreferences.NastaviOperacijoInTip(operacija, tezavnost);
            this.Log = Log.GetInstance();
        }

        public void Run()
        {
            int stevecNapacnihOdgovorov = 0;
            bool run = true;
            while (run)
            {
                switch (UserPreferences.Operacija)
                {
                    case TipOperacije.Seštevanje:
                        if (!Seštevanje(UserPreferences.Težavnost))
                        {
                            stevecNapacnihOdgovorov++;
                        }
                        break;

                    case TipOperacije.Množenje:
                        if (!Produkt(UserPreferences.Težavnost))
                        {
                            stevecNapacnihOdgovorov++;
                        }
                        break;
                }
                if (stevecNapacnihOdgovorov >= 2)
                {
                    run = false;
                }
            }
        }

        private bool Produkt(int težavnost)
        {
            Console.WriteLine("");
            Random rnd = new Random();
            int a = rnd.Next(0, težavnost + 1);
            int b = rnd.Next(0, težavnost + 1);
            Console.Write($"{a} * {b} = ");

            string odg = Console.ReadLine();
            if (int.TryParse(odg, out int odgInt))
            {
                if (int.Parse(odg) == a * b)
                {
                    Console.WriteLine("Odgovor je pravilen.");
                    Log.ZapisiVDatoteko($"Vprašanje: {a} + {b} -> Odgovor je pravilen: {a + b}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Odgovor je nepravilen, pravilni odgovor je: {a * b}");
                    Log.ZapisiVDatoteko($"Vprašanje: {a} + {b} -> Odgovor je nepravilen, pravilni odgovor je: {a + b}");
                    return false;
                }
            }
            Log.ZapisiVDatoteko($"Vprašanje: {a} + {b} -> Odgovor je nepravilen: {a + b}");
            return false;
        }

        private bool Seštevanje(int težavnost)
        {
            Console.WriteLine("");
            Random rnd = new Random();
            int a = rnd.Next(0, težavnost);
            int b = rnd.Next(0, težavnost);
            Console.Write($"{a} + {b} = ");
            string odg = Console.ReadLine();
            if (int.TryParse(odg, out int odgInt))
            {
                if (int.Parse(odg) == a + b)
                {
                    Console.WriteLine("Odgovor je pravilen.");
                    Log.ZapisiVDatoteko($"Vprašanje: {a} + {b} -> Odgovor je pravilen: {a + b}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Odgovor je nepravilen, pravilni odgovor je: {a + b}");
                    Log.ZapisiVDatoteko($"Vprašanje: {a} + {b} -> Odgovor je nepravilen, pravilni odgovor je: {a + b}");
                    return false;
                }
            }
            Log.ZapisiVDatoteko($"Vprašanje: {a} + {b} -> Odgovor je nepravilen: {a + b}");
            return false;
        }
    }

}
