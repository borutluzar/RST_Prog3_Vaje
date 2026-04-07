using System.Net.ServerSentEvents;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public interface IPrinter
    {
        public void Print(string document);
    }

    public interface IScanner
    {
        public string Scan();
    }

    public interface IMultiFunctionDevice : IPrinter, IScanner
    {
        // Uporabimo možnost implementacije funkcije že kar v interface-u
        public void Copy()
        {
            string scanned = Scan();
            Print(scanned);
        }
    }

    public class SmartPrinter : IMultiFunctionDevice
    {
        public void Print(string document)
        {
            Console.WriteLine($"Natisnil {document}");
        }

        public string Scan()
        {
            Console.WriteLine("Dokument je bil skeniran");
            return "Skenirani tekst";
        }

        // Funkcija, ki jo definiramo že v vmesniku, se kliče kar direktno iz vmesnika,
        // ali pa jo prepišemo v razredu - definirati jo moramo eksplicitno!        
        /*void IMultiFunctionDevice.Copy()
        {
            Console.WriteLine("SmartPrinter je uspešno kopiral dokument");
        }*/
    }
}
