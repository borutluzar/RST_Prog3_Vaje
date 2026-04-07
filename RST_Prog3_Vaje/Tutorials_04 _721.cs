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
    interface IDocument
    {
        public string Author { get; set; }
        public DateTime DateCreated { get; }
        public void Sign();
    }

    interface IWorkFlow
    {
        public void Creation(SystemUser responsible);
        public void Validation(SystemUser responsible);
        public void Confirmation(SystemUser responsible);

        public void Sign();
    }

    public class SystemUser
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }

    public class Report : IDocument, IWorkFlow
    {
        public Report(DateTime dtCreated)
        {
            this.DateCreated = dtCreated;
        }

        public string Author
        {
            get; set;
        }

        public DateTime DateCreated { get; }


        //Funkcija za IDocument interface
        public void Sign()
        {
            Console.WriteLine("Poročilo je bilo podpisano.");
        }


        #region IWorkFlow

        public void Confirmation(SystemUser responsible)
        {
            Console.WriteLine($"Poročilo je potrdil {responsible.UserName}");
        }

        public void Creation(SystemUser responsible)
        {
            Console.WriteLine($"Poročilo je ustvaril {responsible.UserName}");
        }

        public void Validation(SystemUser responsible)
        {
            Console.WriteLine($"Poročilo je validiral {responsible.UserName}");
        }

        void IWorkFlow.Sign()
        {
            Console.WriteLine("Poročilo je prišlo preko delovnega toka in je bilo podpisano.");
        }

        #endregion
    }
}
