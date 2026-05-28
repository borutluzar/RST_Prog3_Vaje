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
    public class Tutorials_13
    {
        public enum Exercises
        {
            Exercise_1524 = 1,
            Exercise_1634 = 2
        }

        /// <summary>
        /// Navodila v zapiskih.
        /// </summary>
        public static void Exercise_1524()
        {
            SecureImageProxy imageProxyGuest = new SecureImageProxy(Role.Guest);
            SecureImageProxy imageProxySpecialist = new SecureImageProxy(Role.Specialist);

            MedicalImage imageGuest = imageProxyGuest.GetImageData(1);
            Console.WriteLine("Guest image: " + (imageGuest != null ? imageGuest.ToString() : "slika ni bila pridobljena"));

            MedicalImage imageSpecialist = imageProxySpecialist.GetImageData(1);
            imageProxySpecialist.GetImageData(1);
            Console.WriteLine("image specialist " + imageSpecialist.ToString());

            AnonymizationDecorator anonymization = new AnonymizationDecorator(imageProxySpecialist);
            MedicalImage anonImage = anonymization.GetImageData(1);
            imageSpecialist = imageProxySpecialist.GetImageData(1);
            Console.WriteLine("image specialist " + imageSpecialist.ToString());
            Console.WriteLine("Anonimization " + anonImage.ToString());

            ContrastEnhancementDecorator contrastDecorator = new ContrastEnhancementDecorator(imageProxySpecialist);
            MedicalImage contrastImage = contrastDecorator.GetImageData(1);
            imageSpecialist = imageProxySpecialist.GetImageData(1);
            Console.WriteLine("image specialist " + imageSpecialist.ToString());
            Console.WriteLine("Contrast " + contrastImage.ToString());
        }

        /// <summary>
        /// Navodila v zapiskih.
        /// </summary>
        public static void Exercise_1634()
        {
            CryptoMonitor monitor1 = new CryptoMonitor();
            UserAlertObserver observer1 = new UserAlertObserver(5000);
            UserAlertObserver observer2 = new UserAlertObserver(1500);

            monitor1.OnPriceChecked += observer1.AlertUser;
            monitor1.OnPriceChecked += observer2.AlertUser;

            var report = monitor1.CheckPrice("Doge coin");

            Console.WriteLine(report);
        }
    }


    #region Naloga 15.2.4

    public class MedicalImage
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public List<int> PixelValues { get; set; }
        public bool HasWaterMark { get; set; }

        public override string ToString()
        {
            return $"Name: {PatientName}, Pixel values: {string.Join(", ", this.PixelValues)}, Has Water mark: {HasWaterMark}";
        }
    }

    public interface IMedicalImageProvider
    {
        MedicalImage GetImageData(int patientID);
    }

    public class CentralMedicalStorage : IMedicalImageProvider
    {
        public MedicalImage GetImageData(int patientID)
        {
            var image = new MedicalImage()
            {
                PatientId = patientID,
                PatientName = "Marko Novak",
                PixelValues = new List<int> { 10, 20, 30, 40, 50 },
                HasWaterMark = true
            };
            Console.WriteLine("Pridobivam slike...");
            Thread.Sleep(2000);
            return image;
        }
    }
    public enum Role
    {
        Guest,
        Researcher,
        Specialist
    }
    public class SecureImageProxy : IMedicalImageProvider
    {
        private Role userRole;
        private CentralMedicalStorage storage = new CentralMedicalStorage();
        Dictionary<int, MedicalImage> cache = new Dictionary<int, MedicalImage>();
        public SecureImageProxy(Role role)
        {
            this.userRole = role;
        }

        public MedicalImage? GetImageData(int patientID)
        {
            if (userRole == Role.Guest)
            {
                Console.WriteLine("[Proxy] Dostop je zavrnjen.");
                return null;
            }

            if (cache.ContainsKey(patientID))
            {
                Console.WriteLine("[Proxy] Pridobivam predpomnjeno sliko.");
                return MakeCopy(cache[patientID]);
            }
            else
            {
                Console.WriteLine("[Proxy] Pridobivam slike. Kličem funcijo GetImageData");
                var newImage = storage.GetImageData(patientID);
                cache[patientID] = newImage;

                return MakeCopy(newImage);
            }
        }

        private MedicalImage MakeCopy(MedicalImage image)
        {
            MedicalImage copyImage = new MedicalImage()
            {
                PatientName = image.PatientName,
                PatientId = image.PatientId,
                HasWaterMark = image.HasWaterMark,
                PixelValues = new List<int>(image.PixelValues)
            };
            return copyImage;
        }
    }

    public abstract class MedicalImageDecorator : IMedicalImageProvider
    {
        protected IMedicalImageProvider imageProvider;
        public MedicalImageDecorator(IMedicalImageProvider imageProvider)
        {
            this.imageProvider = imageProvider;
        }

        public abstract MedicalImage GetImageData(int patientID);
    }

    public class AnonymizationDecorator : MedicalImageDecorator
    {
        public AnonymizationDecorator(IMedicalImageProvider imageProvider) : base(imageProvider) { }

        public override MedicalImage GetImageData(int patientID)
        {
            MedicalImage image = this.imageProvider.GetImageData(patientID);
            image.PatientName = string.Empty;
            return image;
        }
    }

    public class ContrastEnhancementDecorator : MedicalImageDecorator
    {
        public ContrastEnhancementDecorator(IMedicalImageProvider imageProvider) : base(imageProvider) { }

        public override MedicalImage GetImageData(int patientID)
        {
            MedicalImage image = imageProvider.GetImageData(patientID);
            image.PixelValues = image.PixelValues.Select(pixel => pixel * 2).ToList();
            return image;
        }
    }

    #endregion


    #region Naloga 16.3.4

    public interface ICryptoDataService
    {
        public decimal GetCurrentPrice(string crypto);
    }

    public class CryptoService : ICryptoDataService
    {
        public decimal GetCurrentPrice(string crypto)
        {
            // Simulacija omrežne zakasnitve (1.5 sekunde)
            Thread.Sleep(1500);
            // Izračun naključne cene kriptovalute
            return 5415.54m * ((decimal)new Random().NextDouble());
        }
    }

    public class CryptoProxy : ICryptoDataService
    {
        // Referenca na dejanski servis, ki ga proxy pokliče le, ko je to potrebno
        private CryptoService apiService = new CryptoService();

        // Predpomnilnik (Cache) narejen z Dictionary-em
        // Ključ: kratica kriptovalute (string)
        // Vrednost: nabor (Tuple), ki vsebuje shranjeno ceno (decimal) in čas zadnje osvežitve (DateTime)
        private Dictionary<string, (decimal Price, DateTime Timestamp)> dicCache = new Dictionary<string, (decimal, DateTime)>();

        // Časovna veljavnost podatkov v predpomnilniku, določena v minutah
        private const int cachingInterval = 10;

        public decimal GetCurrentPrice(string crypto)
        {
            Console.WriteLine($"[Proxy] pridobivamo ceno za {crypto}");
            if (dicCache.ContainsKey(crypto) && (DateTime.Now - dicCache[crypto].Timestamp).TotalMinutes < cachingInterval)
            {
                // če je podatek za kriptovaluto "svež" in ne rabimo pridobitve novega
                return dicCache[crypto].Price;
            }
            else
            {
                // če podatek ne obstaja ali je prestar
                decimal price = apiService.GetCurrentPrice(crypto);
                /*
                 * daljči način ki najprej izbriše podatek iz cacha in nato doda novega
                if (dicCache.ContainsKey(crypto)) {
                    dicCache.Remove(crypto);
                }
                dicCache.Add(crypto, (price, DateTime.Now));
                */

                // krajši način, ki prepiše podatek v cachu
                dicCache[crypto] = (price, DateTime.Now);
                return price;
            }
        }
    }

    // definira event na katerega se lahko "naročijo" opazovalci
    public interface ICryptoMonitor
    {
        event Action<Crypto> OnPriceChecked;
    }

    // objekt ki vsebuje ime in ceno kriptovalute. rabimo ga za pošiljanje preko eventa nastavljenega v ICryptoMonitor-ju
    public struct Crypto
    {
        public string Name { get; }
        public decimal Price { get; }

        public Crypto(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    public class CryptoMonitor : ICryptoMonitor
    {
        private CryptoProxy proxy = new CryptoProxy();
        public event Action<Crypto> OnPriceChecked;

        public CryptoReport CheckPrice(string crypto)
        {
            Console.WriteLine($"[Subject] preverjamo ceno za {crypto}");
            decimal price = proxy.GetCurrentPrice(crypto);
            // Sprožitev dogodka (vsem prijavljenim opazovalcem pošljemo objekt Crypto s svežimi podatki)
            OnPriceChecked?.Invoke(new Crypto(crypto, price));
            CryptoReportBuilder report = new CryptoReportBuilder("Doge coin");
            report.SetPrice(price);
            report.SetHistory(true);
            report.SetChart(true);
            return report.Build();
        }
    }

    // Razred, ki predstavlja konkretnega Opazovalca (Observer) v sistemu
    public class UserAlertObserver
    {
        public decimal SellBound { get; set; }

        public UserAlertObserver(decimal buyBound)
        {
            SellBound = buyBound;
        }

        public void AlertUser(Crypto crypto)
        {
            Console.WriteLine($"[Observer]");
            if (crypto.Price > SellBound)
            {
                Console.WriteLine($"Cena kriptovalute [{crypto.Name}] je: {crypto.Price:c}. Prodajte!!!");
            }
        }
    }

    public interface IReportBuilder
    {
        public void SetPrice(decimal price);
        public void SetHistory(bool history);
        public void SetChart(bool chart);
        public CryptoReport Build();
    }

    public class CryptoReport
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IncludeHistory { get; set; }
        public bool IncludeChart { get; set; }

        public CryptoReport(string name)
        {
            Name = name;
        }

        // Prepisana metoda za strukturiran tekstovni izpis podatkov o zgrajenem objektu.
        public override string ToString()
        {
            return $"Kriptovaluta: {Name} ima ceno {Price}, Zgodovina: {(IncludeHistory ? "Vključena" : "Izključena")}, Graf: {(IncludeChart ? "Vključen" : "Izkjučen")}";
        }
    }

    public class CryptoReportBuilder : IReportBuilder
    {
        private CryptoReport cryptoReport;

        public CryptoReportBuilder(string name)
        {
            this.cryptoReport = new CryptoReport(name);
        }
        public CryptoReport Build()
        {
            return cryptoReport;
        }

        public void SetChart(bool chart)
        {
            this.cryptoReport.IncludeChart = chart;
        }

        public void SetHistory(bool history)
        {
            this.cryptoReport.IncludeHistory = history;
        }

        public void SetPrice(decimal price)
        {
            this.cryptoReport.Price = price;
        }
    }

    #endregion            
}
