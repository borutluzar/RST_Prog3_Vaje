using System.Net.ServerSentEvents;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public class Tutorials_09
    {
        public enum Exercises
        {
            Exercise_821 = 1,
            Exercise_823 = 2,
            Exercise_1421 = 3,
        }

        /// <summary>
        /// Zapišite razširitveno funkcijo za “lep” izpis elementov seznama.
        /// </summary>
        public static void Exercise_821()
        {
            List<string> lstNizov = new() { "Marko", "skače", "po", "zeleni", "travi" };
            Console.WriteLine(lstNizov.ToString<string>());

            List<int> lstStevil = new() { 3, 5, 7, 11, 13, 17, 19 };
            Console.WriteLine(lstStevil.ToString<int>());
        }

        /// <summary>
        /// Zapišite razširitveno funkcijo, ki ugotovi, 
        /// če je dani niz palindrom ali ne.
        /// </summary>
        public static void Exercise_823()
        {
            string word1 = "PericaRežeRaciRep";
            string word2 = "RibaRežeRaciRep";

            Console.WriteLine($"Beseda {word1} {(word1.IsPalindrom() ? "je" : "ni")} palindrom!");
            Console.WriteLine($"Beseda {word2} {(word2.IsPalindrom() ? "je" : "ni")} palindrom!");
        }

        /// <summary>
        /// Imamo vmesnik IMessage s funkcijo Prepare in razred OrdinaryMessage, ki ga implementira. 
        /// S pomočjo vzorca decorator pripravite razrede, 
        /// ki bodo instanco razreda OrdinaryMessage ovili v funkcionalnosti, 
        /// ki bodo sporočilu dodali: 
        /// (a) čas pošiljanja, (b) ga šifrirali (npr.obrnili) in (c) vložili v html značke.
        /// </summary>
        public static void Exercise_1421()
        {
            OrdinaryMessage msg = new OrdinaryMessage();
            Console.WriteLine(msg.Prepare());

            TimeMessageDecorator timeMsg = new TimeMessageDecorator(msg);
            Console.WriteLine(timeMsg.Prepare());

            CodeMessageDecorator codeMsg = new CodeMessageDecorator(msg);
            Console.WriteLine(codeMsg.Prepare());

            HTMLMessageDecorator htmlMsg = new HTMLMessageDecorator(msg);
            Console.WriteLine(htmlMsg.Prepare());

            CodeMessageDecorator codeMsg2 = new CodeMessageDecorator(codeMsg);
            Console.WriteLine(codeMsg2.Prepare());
        }
    }


    #region Nalogi 8.2.1 in 8.2.3
    // Razred za razširitveni funkciji
    public static class Extensions
    {
        /// <summary>
        /// Generična razširitvena funkcija.
        /// </summary>
        public static string ToString<T>(this IEnumerable<T> lst, string separator = ",")
        {
            StringBuilder sb = new StringBuilder();
            // {1, 2, 3}
            sb.Append("{");
            bool first = true;
            foreach (T element in lst)
            {
                if (first)
                {
                    sb.Append(element);
                }
                else
                {
                    sb.Append(separator + " " + element);
                }
                first = false;
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static bool IsPalindrom(this string word)
        {
            string lowerWord = word.ToLower();
            for (int i = 0; i <= word.Length / 2; i++)
            {
                if (lowerWord[i] != lowerWord[word.Length - 1 - i])
                {
                    return false;
                }
            }
            return true;
        }
    }
    #endregion


    #region Naloga 14.2.1

    public interface IMessage
    {
        string Prepare();

    }

    public class OrdinaryMessage : IMessage
    {
        public string Prepare()
        {
            return "Moje sporočilo.";
        }
    }

    public abstract class MessageDecorator : IMessage
    {
        protected IMessage message;

        protected MessageDecorator(IMessage message)
        {

            this.message = message;

        }

        public abstract string Prepare();
    }

    public class TimeMessageDecorator : MessageDecorator
    {
        public TimeMessageDecorator(IMessage message) : base(message) { }

        public override string Prepare()
        {
            return DateTime.Now.ToString("HH:mm:ss") + " " + message.Prepare();
        }

    }

    public class CodeMessageDecorator : MessageDecorator
    {

        public CodeMessageDecorator(IMessage message) : base(message) { }

        public override string Prepare()
        {
            string msg = message.Prepare();
            string sifraMsg = "";

            for (int i = msg.Length - 1; i >= 0; i--)
            {
                sifraMsg += msg[i];
            }

            return sifraMsg;
        }
    }

    public class HTMLMessageDecorator : MessageDecorator
    {
        public HTMLMessageDecorator(IMessage message) : base(message) { }

        public override string Prepare()
        {
            return "<p>" + message.Prepare() + "</p>";
        }
    }

    #endregion
}
