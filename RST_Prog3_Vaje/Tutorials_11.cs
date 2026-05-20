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
    public class Tutorials_11
    {
        public enum Exercises
        {
            Exercise_1631 = 1,
            Exercise_1632 = 2,
            Exercise_1633 = 3
        }

        /// <summary>
        /// Pripravite sistem za obveščcanje o novicah. 
        /// Imate razred NewsSubject, ki objavlja novice različnih kategorij
        /// (šport, politika, zabava).
        /// Ustvarite opazovalce, ki se naročijo le na določene kategorije.
        /// Če se objavi novica iz njihove kategorije, naj prejmejo obvestilo.
        /// </summary>
        public static void Exercise_1631()
        {
            // Z eventi
            NewsSubject news = new NewsSubject();

            NewsObserver obs1 = new NewsObserver();
            NewsObserver obs2 = new NewsObserver();
            NewsObserver obs3 = new NewsObserver();

            news.OnPoliticsNewsAppeared += obs1.ShowNews;
            news.OnSportNewsAppeared += obs1.ShowNews;

            news.OnEntertainmentNewsAppeared += obs2.ShowNews;

            news.OnPoliticsNewsAppeared += obs3.ShowNews;
            news.OnSportNewsAppeared += obs3.ShowNews;
            news.OnEntertainmentNewsAppeared += obs3.ShowNews;

            news.GetNews("hormuška ožina je odprta", "content", NewsCategory.Politics, DateTime.Now);
            news.GetNews("hokeisti osvojili točko", "content", NewsCategory.Sport, DateTime.Now);
            news.GetNews("programiranje 3 imamo samo še 4-krat", "content", NewsCategory.Entertainment, DateTime.Now);

            Console.WriteLine($"\n\tVerzija brez eventov: \n");

            // Brez eventov
            NewsSubject2 sub2 = new NewsSubject2();
            sub2.Subscribe(obs1, NewsCategory.Politics);
            sub2.Subscribe(obs1, NewsCategory.Sport);
            sub2.Subscribe(obs1, NewsCategory.Education);

            sub2.Subscribe(obs2, NewsCategory.Entertainment);

            sub2.Subscribe(obs3, NewsCategory.Politics);
            sub2.Subscribe(obs3, NewsCategory.Sport);
            sub2.Subscribe(obs3, NewsCategory.Entertainment);

            sub2.GetNews("hormuška ožina je odprta", "content", NewsCategory.Politics, DateTime.Now);
            sub2.GetNews("hokeisti osvojili točko", "content", NewsCategory.Sport, DateTime.Now);
            sub2.GetNews("programiranje 3 imamo samo še 4-krat", "content", NewsCategory.Entertainment, DateTime.Now);
            sub2.GetNews("izpit pišemo junija", "content", NewsCategory.Education, DateTime.Now);
        }

        /// <summary>
        /// Implementirajte digitalno banko. Razred BankAccount naj bo subjekt, ki vsebuje lastnost Balance, 
        /// funkciji Deposit(amount) in Withdraw(amount) ter dogodek OnTransaction, 
        /// ki kot argument pošlje tip transakcije, znesek in trenutno stanje. 
        /// Nanj naj bosta naročena dva opazovalca: 
        /// SmsNotifier, ki ob vsaki transakciji izpiše trenutno stanje, 
        /// in SecurityAudit, ki sproži alarm, če stanje na računu pade pod 0 eur.
        /// </summary>
        public static void Exercise_1632()
        {
            BankAccount account = new BankAccount();

            SmsNotifier smsNotifier1 = new SmsNotifier("5132542");
            SecurityAudit auditNotifier1 = new SecurityAudit();

            account.OnTransaction += smsNotifier1.NotifyUser;
            account.OnTransaction += auditNotifier1.NotifyUserAlarm;

            account.Deposit(500);
            account.Withdraw(510);
        }

        /// <summary>
        /// V pametnem domu imamo senzor gibanja. 
        /// Ko senzor zazna gibanje, se mora v hiši zgoditi več stvari hkrati: 
        /// prižgejo se luči, kamera začne snemati, lastnik pa prejme obvestilo na telefon. 
        /// Implementirajte razred MotionSensor z dogodkom OnMotionDetected, 
        /// ki kot argument pošlje lokacijo in čas zaznave. 
        /// Implementirajte opazovalca SmartLight, ki ima funkcijo za prižig luči, 
        /// opazovalca SecurityCamera, ki ima funkcijo za snemanje, 
        /// in opazovalca MobileApp, ki pošlje sms z obvestilom. 
        /// V funkciji Main simulirajte zaznavo gibanja v dnevni sobi in kopalnici.
        /// </summary>
        public static void Exercise_1633()
        {
            /* Čakamo Pavlovo kodo... */
        }
    }


    #region Naloga 16.3.1 - z eventi

    public enum NewsCategory
    {
        Sport,
        Politics,
        Entertainment,
        Education
    }

    public interface INews
    {
        event Action<NewsArticle> OnSportNewsAppeared;
        event Action<NewsArticle> OnPoliticsNewsAppeared;
        event Action<NewsArticle> OnEntertainmentNewsAppeared;
    }

    public class NewsArticle
    {
        public string Title { get; }
        public string Content { get; set; }
        public NewsCategory Category { get; set; }

        public DateTime PublishedDate { get; set; }

        public override string ToString()
        {
            return $"{Title}, ({PublishedDate:dd. MM. yyyy})" +
                $"\n{Content}";
        }

        public NewsArticle(string title, string content, NewsCategory category, DateTime publishedDate)
        {
            this.Title = title;
            this.Content = content;
            this.Category = category;
            this.PublishedDate = publishedDate;
        }
    }

    public class NewsSubject : INews
    {
        public event Action<NewsArticle> OnSportNewsAppeared;
        public event Action<NewsArticle> OnPoliticsNewsAppeared;
        public event Action<NewsArticle> OnEntertainmentNewsAppeared;

        public void GetNews(string title, string content, NewsCategory category, DateTime publishedDate)
        {
            var article = new NewsArticle(title, content, category, publishedDate);
            switch (category)
            {
                case NewsCategory.Sport:
                    {
                        OnSportNewsAppeared.Invoke(article);
                    }
                    break;
                case NewsCategory.Politics:
                    {
                        OnPoliticsNewsAppeared.Invoke(article);
                    }
                    break;
                case NewsCategory.Entertainment:
                    {
                        OnEntertainmentNewsAppeared.Invoke(article);
                    }
                    break;
            }
        }
    }

    // Ta razred uporabimo v obeh verzijah,
    // v drugi dodamo INewsObserver
    public class NewsObserver : INewsObserver
    {
        public void ShowNews(NewsArticle article)
        {
            Console.WriteLine($"[Observer] {article}");
        }
    }

    #endregion

    #region Naloga 16.3.1 - brez eventov

    public interface INewsSubject
    {
        void Subscribe(INewsObserver observer, NewsCategory category);
        void Unsubscribe(INewsObserver observer, NewsCategory category);
        void NotifyAll(NewsCategory category);
    }

    public interface INewsObserver
    {
        void ShowNews(NewsArticle article);
    }

    public class NewsSubject2 : INewsSubject
    {
        Dictionary<NewsCategory, List<INewsObserver>> dicObservers = new Dictionary<NewsCategory, List<INewsObserver>>();
        private NewsArticle article;
        public NewsSubject2()
        {
            foreach (NewsCategory category in Enum.GetValues(typeof(NewsCategory)))
            {
                dicObservers[category] = new List<INewsObserver>();
            }
        }
        public void GetNews(string title, string content, NewsCategory category, DateTime publishedDate)
        {
            var article = new NewsArticle(title, content, category, publishedDate);
            this.article = article;
            NotifyAll(category);

        }
        public void NotifyAll(NewsCategory category)
        {
            foreach (var observer in dicObservers[category])
            {
                observer.ShowNews(this.article);
            }
        }

        public void Subscribe(INewsObserver observer, NewsCategory category)
        {
            dicObservers[category].Add(observer);
        }

        public void Unsubscribe(INewsObserver observer, NewsCategory category)
        {
            dicObservers[category].Remove(observer);
        }
    }

    #endregion


    #region Naloga 16.3.2

    public interface IBankTransaction
    {
        public event Action<string, double, double> OnTransaction;
    }

    public class BankAccount : IBankTransaction
    {
        public event Action<string, double, double>? OnTransaction;

        public double Balance { get; private set; }

        public void Deposit(double amount)
        {
            this.Balance += amount;
            this.OnTransaction?.Invoke("Deposit", amount, this.Balance);
        }

        public void Withdraw(double amount)
        {
            this.Balance -= amount;
            this.OnTransaction?.Invoke("Withdraw", amount, this.Balance);
        }
    }

    public class SmsNotifier
    {
        private string phoneNumber;

        public SmsNotifier(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void NotifyUser(string transactionType, double amount, double balance)
        {
            Console.WriteLine($"Pošiljam obvestilo na številko {phoneNumber}, transakcija vrste {transactionType}-> znesek:{amount}, stanje:{balance}");
        }
    }

    public class SecurityAudit
    {
        private const double minBalance = 0;

        public void NotifyUserAlarm(string transactionType, double amount, double balance)
        {
            if (transactionType == "Withdraw" && balance < minBalance)
            {
                Console.WriteLine($"POZOR!!! Vaše stanje je padlo pod {minBalance}!");
            }
        }
    }

    #endregion


    #region Naloga 16.3.3


    #endregion
}
