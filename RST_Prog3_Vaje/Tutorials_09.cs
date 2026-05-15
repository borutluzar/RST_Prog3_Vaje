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
    public class Tutorials_09
    {
        public enum Exercises
        {
            Exercise_823 = 1,
            Exercise_1421 = 2,
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Exercise_823()
        {
            string word1 = "PericaRežeRaciRep";
            string word2 = "RibaRežeRaciRep";

            Console.WriteLine($"Beseda {word1} {(word1.IsPalindrom() ? "je" : "ni")} palindrom!");
            Console.WriteLine($"Beseda {word2} {(word2.IsPalindrom() ? "je" : "ni")} palindrom!");
        }

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


    #region Naloga 8.2.3
    // Razred za razširitveno funkcijo
    public static class Palindrom
    {
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
        public TimeMessageDecorator(IMessage message) : base(message)  {  }

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
        public HTMLMessageDecorator(IMessage message) : base(message) {  }

        public override string Prepare()
        {
            return "<p>" + message.Prepare() + "</p>";
        }
    }

    #endregion
}
