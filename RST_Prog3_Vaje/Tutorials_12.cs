using System.Net.ServerSentEvents;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public class Tutorials_12
    {
        public enum Exercises
        {
            Exercise_1721 = 1,
            Exercise_1722 = 2,
            Exercise_1723 = 3
        }

        /// <summary>
        /// Pripravite razredni model, ki bo opisoval atlete. 
        /// Atlet naj bo nadrazred, ki ima nekaj podrazredov (npr.metalec, šprinter, skakalec, deseterobojec). 
        /// V glavnem razredu določite nekaj ustreznih funkcionalnosti (tek, met, skok), 
        /// ki jih podrazredom ločeno implementirate glede na njihove specifike.
        /// Uporabite vzorec strategy.
        /// </summary>
        public static void Exercise_1721()
        {
            Runner runner1 = new Runner(new Sprint());
            Runner runner2 = new Runner(new LongDistance());

            Thrower thrower1 = new Thrower(new HammerThrow());
            Thrower thrower2 = new Thrower(new DiscusThrow());

            Jumper jumper1 = new Jumper(new LongJump());
            Jumper jumper2 = new Jumper(new HighJump());

            Console.WriteLine($"Atlet {nameof(runner1)}");
            runner1.Run();
            runner1.Throw();
            runner1.Jump();

            Console.WriteLine($"Atlet {nameof(runner2)}");
            runner2.Run();
            runner2.Throw();
            runner2.Jump();

            Console.WriteLine($"Atlet {nameof(thrower1)}");
            thrower1.Run();
            thrower1.Throw();
            thrower1.Jump();

            Console.WriteLine($"Atlet {nameof(thrower2)}");
            thrower2.Run();
            thrower2.Throw();
            thrower2.Jump();

            Console.WriteLine($"Atlet {nameof(jumper1)}");
            jumper1.Run();
            jumper1.Throw();
            jumper1.Jump();

            Console.WriteLine($"Atlet {nameof(jumper2)}");
            jumper2.Run();
            jumper2.Throw();
            jumper2.Jump();
        }

        /// <summary>
        /// V spletni trgovini ob zaključku nakupa izberemo dostavljalca, 
        /// sistem pa izračuna strošek dostave na podlagi teže paketa. 
        /// Definirajte vmesnik IShippingStrategy s funkcijo ComputeDeliveryRate 
        /// in zanj ustvarite tri konkretne razrede s strategijami: 
        /// SloveniaPostStrategy, DhlExpressStrategy in LocalPickupStrategy. 
        /// Ustvarite še glavni razred ShippingCalculator z lastnostjo ShippingStrategy, 
        /// kateri lahko vrednost nastavimo povsod. 
        /// Ceno dostave nam naj vrne funkcija GetFinalPrice.
        /// </summary>
        public static void Exercise_1722()
        {
            ShippingCalculator calcSloPost = new ShippingCalculator();
            calcSloPost.SetStrategy(new SloveniaPostStrategy());

            ShippingCalculator calcDhl = new ShippingCalculator();
            calcDhl.SetStrategy(new DhlExpressStrategy());

            double weight = 2.5;
            Console.WriteLine($"Cena pri SLO pošti za težo {weight} je {calcSloPost.GetFinalPrice(weight)}");
            Console.WriteLine($"Cena pri DHL za težo {weight} je {calcDhl.GetFinalPrice(weight)}");            
        }

        /// <summary>
        /// Pripravite abstrakten razred mobilna naprava, 
        /// zanjo naredite nekaj podrazredov in implementirajte funkcionalnosti zanje.
        /// Npr.pošiljanje SMS-ov, telefoniranje, sprejemanje signala 4G ali celo 5G.
        /// Naprave naj bodo med seboj karseda različne, obenem pa naj model omogoča 
        /// preprosto dopolnjevanje dodatnih funkcionalnosti in dodajanje novih podrazredov.
        /// Uporabite vzorec strategy.
        /// </summary>
        public static void Exercise_1723()
        {
            SmartPhone smartPhone1 = new(1);
            SmartPhone smartPhone2 = new(2);

            smartPhone1.SendSMS("Dober dan.", 2);
            smartPhone2.Call(1);
        }
    }


    #region Naloga 17.2.1

    public interface IRunStrategy
    {
        void Run();
    }

    public interface IThrowStrategy
    {
        void Throw();
    }

    public interface IJumpStrategy
    {
        void Jump();
    }

    public abstract class Athlete
    {
        public IRunStrategy RunStrategy { get; set; }
        public IThrowStrategy ThrowStrategy { get; set; }
        public IJumpStrategy JumpStrategy { get; set; }

        public void Run()
        {
            this.RunStrategy?.Run();
        }

        public void Throw()
        {
            this.ThrowStrategy?.Throw();
        }

        public void Jump()
        {
            this.JumpStrategy?.Jump();
        }
    }

    public class Runner : Athlete
    {
        public Runner(IRunStrategy runStrat)
        {
            this.RunStrategy = runStrat;
        }
    }

    public class Jumper : Athlete
    {
        public Jumper(IJumpStrategy jumpStrat)
        {
            this.JumpStrategy = jumpStrat;
        }
    }

    public class Thrower : Athlete
    {
        public Thrower(IThrowStrategy throwStrat)
        {
            this.ThrowStrategy = throwStrat;
        }
    }

    public class Sprint : IRunStrategy
    {
        public void Run()
        {
            Console.WriteLine($"Tečem zelo hitro");
        }
    }

    public class LongDistance : IRunStrategy
    {
        public void Run()
        {
            Console.WriteLine($"Tečem zelo dolgo");
        }
    }

    public class DiscusThrow : IThrowStrategy
    {
        public void Throw()
        {
            Console.WriteLine($"Mečem disk");
        }
    }

    public class HammerThrow : IThrowStrategy
    {
        public void Throw()
        {
            Console.WriteLine($"Mečem kladivo");
        }
    }

    public class HighJump : IJumpStrategy
    {
        public void Jump()
        {
            Console.WriteLine($"Skačem visoko");
        }
    }

    public class LongJump : IJumpStrategy
    {
        public void Jump()
        {
            Console.WriteLine($"Skačem daleč");
        }
    }


    #endregion


    #region Naloga 17.2.2

    public interface IShippingStrategy
    {
        public double ComputeDeliveryRate(double weight);
    }

    public class SloveniaPostStrategy : IShippingStrategy
    {
        public double ComputeDeliveryRate(double weight)
        {
            return 1.50 + weight * 0.23;
        }
    }

    public class DhlExpressStrategy : IShippingStrategy
    {
        public double ComputeDeliveryRate(double weight)
        {
            return 2.00 + weight * 0.3;
        }
    }

    public class LocalPickupStrategy : IShippingStrategy
    {
        public double ComputeDeliveryRate(double weight)
        {
            return 0.0;
        }
    }

    public class ShippingCalculator
    {
        public IShippingStrategy ShippingStrategy { get; private set; }

        public void SetStrategy(IShippingStrategy ss)
        {
            this.ShippingStrategy = ss;
        }

        public double GetFinalPrice(double weight)
        {
            return this.ShippingStrategy.ComputeDeliveryRate(weight);
        }
    }

    #endregion


    #region Naloga 17.2.3

    public abstract class MobileDevice
    {
        public int ID { get; }
        protected ISMSStrategy? SMSStrategy { get; set; }
        protected ICallingStrategy? CallingStrategy;
        protected IMobileData? MobileDataStrategy;

        public MobileDevice(int id)
        {
            this.ID = id;
        }

        public void SendSMS(string message, int receiverID)
        {
            SMSStrategy?.SendSMS(message, this.ID, receiverID);
        }

        public void Call(int receiverID)
        {
            CallingStrategy?.Call(this.ID, receiverID);
        }

        public void ReceiveSignal()
        {
            MobileDataStrategy?.ReceiveSignal(this.ID);
        }
    }

    public class SmartPhone : MobileDevice
    {
        public SmartPhone(int id) : base(id)
        {
            SMSStrategy = new SMSStrategy();
            CallingStrategy = new CallingSateliteStrategy();
            MobileDataStrategy = null;
        }
    }


    public interface ISMSStrategy
    {
        public void SendSMS(string message, int senderID, int receiverID);
    }

    public interface ICallingStrategy
    {
        void Call(int callerID, int receiverID);
    }

    public interface IMobileData
    {
        void ReceiveSignal(int deviceID);
    }

    class SMSStrategy : ISMSStrategy
    {
        public void SendSMS(string message, int senderID, int receiverID)
        {
            Console.WriteLine($"{senderID} je poslal sporočilo '{message}' prejemniku {receiverID}");
        }
    }

    class CallingGroundStrategy : ICallingStrategy
    {
        public void Call(int callerID, int receiverID)
        {
            Console.WriteLine($"{callerID} je poklical {receiverID} z navadnega telefona");
        }
    }

    class CallingSateliteStrategy : ICallingStrategy
    {
        public void Call(int callerID, int receiverID)
        {
            Console.WriteLine($"{callerID} je poklical {receiverID} s satelitskega telefona");
        }
    }

    #endregion
}
