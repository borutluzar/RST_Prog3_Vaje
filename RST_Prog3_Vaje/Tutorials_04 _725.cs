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
    public interface ITicketReservation
    {
        public DateTime Departure { get; }

        public void Reserve(int ticketID);
    }

    public interface ISeatReservation
    {
        public int SeatNumber { get; set; }

        public bool IsWindowSeat { get; }
        public void Reserve(int seatID);
    }

    public class Ticket
    {
        public int ID { get; }

        public int SeatID { get; }

        public Ticket(int id, int seatId)
        {
            this.ID = id;

            this.SeatID = seatId;
        }
    }

    public class PlaneTravel : ITicketReservation, ISeatReservation
    {
        public DateTime Departure { get; }

        public int SeatNumber { get; set; }

        public bool IsWindowSeat { get; }

        void ITicketReservation.Reserve(int ticketID)
        {
            Console.WriteLine($"Rezervacija je opravljena za karto {ticketID}");
        }

        void ISeatReservation.Reserve(int seatID)
        {
            Console.WriteLine($"Rezervacija je opravljena za sedež {seatID}");
        }

        public Ticket Ticket { get; set; }
    }
}
