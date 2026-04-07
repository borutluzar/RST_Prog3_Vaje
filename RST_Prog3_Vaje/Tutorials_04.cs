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
    public class Tutorials_04
    {
        public enum Exercises
        {
            Exercise_721 = 1,
            Exercise_722 = 2,
            Exercise_725 = 3
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
        public static void Exercise_721()
        {
            Report porocilo = new Report(DateTime.Now);
            porocilo.Author = "Peter";
            
            SystemUser suTone = new SystemUser() { UserName = "Tone", Role = "Tajnik"};
            SystemUser suTine = new SystemUser() { UserName = "Tine", Role = "Pravnik" };
            SystemUser suBine = new SystemUser() { UserName = "Bine", Role = "Direktor" };

            porocilo.Creation(suTone);
            porocilo.Validation(suTine);
            porocilo.Confirmation(suBine);
        }

        /// <summary>
        /// Vaše podjetje razvija programsko opremo za različne pisarniške naprave. 
        /// Imate naprave, ki samo tiskajo, naprave, ki samo skenirajo, in napredne večopravilne naprave, 
        /// ki zmorejo oboje in še več (npr.kopiranje).
        /// Ustvarite vmesnik IPrinter s funkcijo Print(string document), 
        /// vmesnik IScanner s funkcijo Scan(), ki vrača string (poskenirano vsebino), 
        /// in vmesnik IMultiFunctionDevice, ki deduje od IPrinter in IScanner.
        /// Ta vmesnik naj doda še svojo funkcijo Copy(). 
        /// Ustvarite razred SmartPrinter, ki implementira vmesnik IMultiFunctionDevice. 
        /// Pri funkciji Copy() naj naprava najprej uporabi skener, nato pa rezultat pošlje tiskalniku. 
        /// Delovanje preizkusite v funkciji Main.
        /// </summary>
        public static void Exercise_722()
        {
            SmartPrinter tiskalnik = new SmartPrinter();
            tiskalnik.Scan();
            tiskalnik.Print("Besedilo");

            // Funkcijo moramo poklicati eksplicitno iz vmesnika
            ((IMultiFunctionDevice)tiskalnik).Copy();
        }

        /// <summary>
        /// Pripravite dva vmesnika. Prvi naj definira funkcije, ki jih potrebuje razred, 
        /// ki omogoča nakup oziroma rezervacijo vozne karte za poljuben tip prevoza.
        /// Drugi vmesnik naj vsebuje funkcije, ki omogočajo rezervacijo ustreznega sedišča na prevoznem sredstvu.
        /// Oba vmesnika naj vsebujeta funkcijo Reserve, ki kot parameter dobi 
        /// identifikacijsko številko karte oziroma sedeža.
        /// Nato implementirajte razred, npr.PlaneTravel, ki bo poskrbel za funkcionalnosti, 
        /// ki jih mora imeti sistem za rezervacijo vozne karte in ustreznega sedišča. 
        /// Za potrebe naloge definirajte tudi razred Ticket, ki predstavlja vozno karto. 
        /// Na koncu še kreirajte instanco tega objekta in pokličite vse njegove funkcije.
        /// </summary>
        public static void Exercise_725()
        {
            PlaneTravel let = new PlaneTravel();
            Ticket karta = new Ticket(1, 2);
            let.Ticket = karta;

            ((ITicketReservation)let).Reserve(karta.ID);
            ((ISeatReservation)let).Reserve(karta.SeatID);
        }
    }

}
