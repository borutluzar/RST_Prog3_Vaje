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
    public class Tutorials_15
    {
        public enum Exercises
        {
            Exercise_1725 = 1,
        }

        /// <summary>
        /// Navodila v zapiskih.
        /// </summary>
        public static void Exercise_1725()
        {
            User uporbnik = new User()
            {
                Wallet = new Wallet(10_000),
                UserRole = UserRole.Basic,
                ID = 1
            };

            SecureOrderExecutionProxy secureOrderExecutionProxy = new SecureOrderExecutionProxy(uporbnik, 5);
            //OrderFactory.CreateOrder(OrderType.Buy, "BTC", 5_000);

            BuyOrder narociloKupi1 = new BuyOrder("BTC", 6_000, 0.01m);
            narociloKupi1.SetTradingStrategy(new AggressiveStrategy());

            BuyOrder narociloKupi2 = new BuyOrder("DGC", 5_000, 0.01m);
            narociloKupi2.SetTradingStrategy(new ConservativeStrategy());

            SellOrder narociloProdaj1 = new SellOrder("BTC", 7_000);
            narociloProdaj1.SetTradingStrategy(new AggressiveStrategy());

            SellOrder narociloProdaj2 = new SellOrder("BTC", 5_000);
            narociloProdaj2.SetTradingStrategy(new ConservativeStrategy());

            secureOrderExecutionProxy.ProcessOrder(narociloKupi1);
            secureOrderExecutionProxy.ProcessOrder(narociloKupi2);
            secureOrderExecutionProxy.ProcessOrder(narociloProdaj1);
            secureOrderExecutionProxy.ProcessOrder(narociloProdaj2);
        }
    }


    #region Naloga 17.2.5

    public class TransactionResult
    {
        public string Currency { get; }
        public OrderType Type { get; }
        public decimal CurrencyQuantity { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Provision { get; set; }

        public TransactionResult(string currency, OrderType type)
        {
            this.Currency = currency;
            this.Type = type;
        }
    }

    public interface IOrder
    {
        public string KryptoCurrency { get; }
        public decimal Amount { get; }
    }

    public abstract class Order : IOrder
    {
        public string KryptoCurrency { get; }
        public decimal Amount { get; set; }
        public OrderType OrderType { get; protected set; }
        public ITradingStrategy TradingStrategy { get; protected set; }

        public abstract void SetTradingStrategy(ITradingStrategy tradingStrategy);

        internal Order(string kryptoCurrency, decimal amount)
        {
            this.KryptoCurrency = kryptoCurrency;
            this.Amount = amount;
        }

        public TransactionResult Execute()
        {
            return this.TradingStrategy.ExecuteOrder(this);
        }
    }

    public class BuyOrder : Order
    {
        private decimal tax;
        internal BuyOrder(string kryptoCurrency, decimal amount, decimal tax) : base(kryptoCurrency, amount)
        {
            this.OrderType = OrderType.Buy;
            this.tax = tax;
        }

        public override void SetTradingStrategy(ITradingStrategy tradingStrategy)
        {
            this.TradingStrategy = new TaxDecorator(tradingStrategy, tax);
        }

        public override string ToString()
        {
            return ($"Nakupno naročilo: {KryptoCurrency}-> {Amount: 0.00}");
        }
    }

    public class SellOrder : Order
    {
        private decimal provisionReduction;
        internal SellOrder(string kryptoCurrency, decimal amount, decimal provisionReduction = 0) : base(kryptoCurrency, amount)
        {
            this.OrderType = OrderType.Sell;
            this.provisionReduction = provisionReduction;
        }

        public override void SetTradingStrategy(ITradingStrategy tradingStrategy)
        {
            this.TradingStrategy = new PremiumTradeDecorator(tradingStrategy, provisionReduction);
        }

        public override string ToString()
        {
            return ($"Prodajno naročilo: {KryptoCurrency}-> {Amount: 0.00}");
        }
    }

    public enum OrderType
    {
        Buy,
        Sell

    }

    public class OrderFactory
    {
        public static Order CreateOrder(OrderType type, string kryptoCurrency, decimal amount, decimal tax = 0, decimal provisionReduction = 0)
        {
            switch (type)
            {
                case OrderType.Buy:
                    return new BuyOrder(kryptoCurrency, amount, tax);


                case OrderType.Sell:
                    return new SellOrder(kryptoCurrency, amount, provisionReduction);

                default:
                    throw new Exception("Ta tip naročila ni podprt");

            }
        }
    }

    public interface ITradingStrategy
    {
        public TransactionResult ExecuteOrder(Order order);
    }

    public class AggressiveStrategy : ITradingStrategy
    {
        public TransactionResult ExecuteOrder(Order order)
        {
            Console.WriteLine($"Izvedeno je bilo naročilo: {order}");
            return new TransactionResult(order.KryptoCurrency, order.OrderType)
            {
                TransactionAmount = order.Amount
            };
        }
    }

    public class ConservativeStrategy : ITradingStrategy
    {
        public TransactionResult ExecuteOrder(Order order)
        {
            Console.WriteLine("[ConservativeStrategy.ExecuteOrder] Čakamo na ustrezno ceno");
            Thread.Sleep(2000);
            Console.WriteLine($"Izvedeno je bilo naročilo: {order}");
            return new TransactionResult(order.KryptoCurrency, order.OrderType)
            {
                TransactionAmount = order.Amount
            };
        }
    }

    public abstract class OrderDecorator : ITradingStrategy
    {
        protected ITradingStrategy TradingStrategy { get; set; }
        public OrderDecorator(ITradingStrategy tradingStrategy)
        {
            this.TradingStrategy = tradingStrategy;
        }
        public abstract TransactionResult ExecuteOrder(Order order);
    }

    public class TaxDecorator : OrderDecorator
    {
        private decimal taxRate;
        public TaxDecorator(ITradingStrategy tradingStrategy, decimal taxRate) : base(tradingStrategy)
        {
            this.taxRate = taxRate;
        }

        public override TransactionResult ExecuteOrder(Order order)
        {
            TransactionResult result = this.TradingStrategy.ExecuteOrder(order);
            result.Tax = taxRate * result.TransactionAmount;
            return result;
        }
    }

    public class PremiumTradeDecorator : OrderDecorator
    {
        private decimal provisionReduction;

        public PremiumTradeDecorator(ITradingStrategy tradingStrategy, decimal provisionReduction) : base(tradingStrategy)
        {
            this.provisionReduction = provisionReduction;
        }

        public override TransactionResult ExecuteOrder(Order order)
        {
            TransactionResult result = this.TradingStrategy.ExecuteOrder(order);
            result.Provision -= provisionReduction;
            return result;
        }
    }

    public interface IOrderProcessor
    {
        public void ProcessOrder(Order order);
    }

    public class OrderProcessor : IOrderProcessing, IOrderProcessor
    {
        public event Action<TransactionResult>? OnOrderProcessed;
        private decimal provision;
        public OrderProcessor(decimal provision)
        {
            this.provision = provision;
        }
        public void ProcessOrder(Order order)
        {
            order.Amount -= this.provision;
            TransactionResult result = order.Execute();
            result.Provision += this.provision;

            this.OnOrderProcessed?.Invoke(result);
        }
    }

    public interface IOrderProcessing
    {
        public event Action<TransactionResult> OnOrderProcessed;
    }

    public class Wallet
    {
        public Dictionary<string, decimal> Portfolio { get; set; }

        public decimal FiatAmount { get; set; }

        public Wallet(decimal fiatAmount)
        {
            this.FiatAmount = fiatAmount;

            Portfolio = new();
        }
    }

    public class WalletObserver
    {
        private Wallet wallet;
        public WalletObserver(Wallet wallet)
        {
            this.wallet = wallet;
        }
        public void UpdateWallet(TransactionResult transactionResult)
        {
            Console.WriteLine($"Posodabljam Denarnico. Preostali znesek je: {wallet.FiatAmount}");
        }
    }

    public class AuditLogger
    {
        public void LogTransaction(TransactionResult transactionResult)
        {
            Console.WriteLine("V dnevnik transakcij, dajem transakcijo");
        }
    }

    public class SecureOrderExecutionProxy : IOrderProcessor
    {
        public User User { get; }

        private OrderProcessor orderProcessor;

        public SecureOrderExecutionProxy(User user, decimal provision)
        {
            this.User = user;

            orderProcessor = new OrderProcessor(provision);

            WalletObserver walletObserver = new WalletObserver(User.Wallet);

            orderProcessor.OnOrderProcessed += walletObserver.UpdateWallet;
        }
        public void ProcessOrder(Order order)
        {
            if (User.UserRole == UserRole.Guest)
            {
                Console.WriteLine("[SecureOrderExecutionProxy] Uporabnik nima ustrezne vloge.");
                return;
            }

            if (order.OrderType == OrderType.Buy)
            {
                if (User.Wallet.FiatAmount >= order.Amount)
                {
                    User.Wallet.FiatAmount -= order.Amount;
                    orderProcessor.ProcessOrder(order);
                }
                else
                {
                    Console.WriteLine("Nimate dovolj sredstev za izvedbo naročila.");
                }
            }
            else
            {
                if (User.Wallet.Portfolio.ContainsKey(order.KryptoCurrency) && User.Wallet.Portfolio[order.KryptoCurrency] >= order.Amount)
                {
                    User.Wallet.FiatAmount += order.Amount;
                    orderProcessor.ProcessOrder(order);
                }
                else
                {
                    Console.WriteLine("Nimate dovolj valute v denarnici za izvedbo naročila.");
                }
            }
        }
    }

    public class User
    {
        public Wallet Wallet { get; set; }
        public int ID { get; set; }
        public UserRole UserRole { get; set; }
    }

    public enum UserRole
    {
        Guest,
        Basic,
        Premium,
        Admin
    }

    #endregion
}
