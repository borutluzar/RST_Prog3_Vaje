using System.Net.ServerSentEvents;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public class Tutorials_10
    {
        public enum Exercises
        {
            Exercise_1521 = 1, 
            Exercise_1522 = 2,
            Exercise_1523 = 3
        }

        /// <summary>
        /// Imamo dostop do podatkov, ki jih s pomočjo API-ja pošilja vremenska hišica.
        /// Funkcije API-ja so definirane v vmesniku IWeatherData, pošiljajo pa temperaturo, 
        /// zračni tlak, hitrost vetra in količino padavin. 
        /// Pripravite simulacijo razreda, ki bo implementiral zgornji vmesnik 
        /// in pošiljal samo vrednosti posameznih senzorjev. 
        /// Pripravite še dva proxy razreda. Prvi naj podatke za dano vremensko hišico obdela in izpiše v bolj berljivi obliki, 
        /// drugi pa naj podatke oblikuje v JSON zapise.
        /// </summary>
        public static void Exercise_1521()
        {
            WeatherDataProxy dataProxy = new WeatherDataProxy();
            Console.WriteLine($"Izpis z WeatherDataProxy:\n" +
                $"{dataProxy.AirPressure()}");
            Console.WriteLine($"Izpis z WeatherDataProxy:\n" +
                $"{dataProxy.Temperature()}");
            Console.WriteLine($"Izpis z WeatherDataProxy:\n" +
                $"{dataProxy.WindSpeed()}");
            Console.WriteLine($"Izpis z WeatherDataProxy:\n" +
                $"{dataProxy.Precipitation()}");

            WeatherJsonDataProxy dataJson = new WeatherJsonDataProxy();
            Console.WriteLine($"Izpis z WeatherJsonDataProxy:\n" +
                $"{dataJson.AirPressure()}");
            Console.WriteLine($"Izpis z WeatherJsonDataProxy:\n" +
                $"{dataJson.Temperature()}");
            Console.WriteLine($"Izpis z WeatherJsonDataProxy:\n" +
                $"{dataJson.WindSpeed()}");
            Console.WriteLine($"Izpis z WeatherJsonDataProxy:\n" +
                $"{dataJson.Precipitation()}");
        }

        /// <summary>
        /// Pripravite primer virtualnega proxy razreda za primer knjig v knjižnici. 
        /// Knjiga naj implementira vmesnik IBook s funkcijo ShowContent. 
        /// Implementirajte osnovni razred za prave knjige, ki naj v konstruktorju naloži njeno vsebino, 
        /// za kar potrebuje toliko milisekund, kot ima knjiga strani. 
        /// V funkciji ShowContent nato vsebino prikaže. 
        /// Pripravite še proxy razred, ki za začetek dobi naslov knjige in število strani, 
        /// vsebine pa ne naloži. Naloži naj jo šele, ko jo želimo prikazati.
        /// </summary>
        public static void Exercise_1522()
        {
            List<IBook> lstBooks = new();
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                lstBooks.Add(new BookProxy("Naslov_" + i, rnd.Next(300, 1600)));
            }
            Console.WriteLine("Knjige so naložene");

            lstBooks[13].ShowContent();
            lstBooks[63].ShowContent();
        }

        /// <summary>
        /// Pripravite primer varnostnega proxy razreda za naslednjo nalogo. 
        /// V podjetju imamo razred EmployeesData, ki vsebuje funkcijo GetSalary, 
        /// ki vrne plačo podanega zaposlenega (podamo ga npr. z njegovo davčno številko). 
        /// Pripravite proxy razred, ki bo plačo zaposlenega prikazal samo v primeru, 
        /// ko ima poizvedovalec ustrezno vlogo (imamo vloge administrator, uporabnik, gost).
        /// </summary>
        public static void Exercise_1523()
        {
            EmployeesData data = new EmployeesData();
            Console.WriteLine($"Klic funkcije neposredno iz glavnega razreda: " +
                $"\n{data.GetSalary("69542141")}");

            EmployeesDataProxy dataProxy1 = new EmployeesDataProxy(EmployeeRole.Administrator);
            Console.WriteLine($"Klic funkcije neposredno iz proxy razreda z vlogo Administrator: " +
                $"\n{dataProxy1.GetSalary("69542141")}");

            EmployeesDataProxy dataProxy2 = new EmployeesDataProxy(EmployeeRole.User);
            Console.WriteLine($"Klic funkcije neposredno iz proxy razreda z vlogo User: " +
                $"\n{dataProxy2.GetSalary("69542141")}");
        }
    }


    #region Naloga 15.2.1

    public interface IWeatherData
    {
        public string AirPressure();

        public string Temperature();

        public string WindSpeed();

        // količina padavin
        public string Precipitation();
    }

    public class WeatherData : IWeatherData
    {
        public string AirPressure()
        {
            return "" + new Random().Next(800, 1201);
        }

        public string Precipitation()
        {
            return "" + new Random().Next(0, 40);
        }

        public string Temperature()
        {
            return "" + new Random().NextDouble(-10, 30, 1);
        }

        public string WindSpeed()
        {
            return "" + new Random().NextDouble(0, 20, 1);
        }
    }

    public class WeatherDataProxy : IWeatherData
    {
        private readonly WeatherData weatherData = new WeatherData();

        public string AirPressure()
        {
            return "Zračni tlak je: " + weatherData.AirPressure();
        }

        public string Precipitation()
        {
            return "Količina padavin je: " + weatherData.Precipitation();
        }

        public string Temperature()
        {
            return "Temperatura je: " + weatherData.Temperature();
        }

        public string WindSpeed()
        {
            return "Hitrost vetra je: " + weatherData.WindSpeed();
        }
    }

    public class WeatherJsonDataProxy : IWeatherData
    {
        private readonly WeatherData weatherData = new WeatherData();

        public string AirPressure()
        {
            return "{AirPressure: " + weatherData.AirPressure() + "}";
        }

        public string Precipitation()
        {
            return "{Precipitation: " + weatherData.Precipitation() + "}";
        }

        public string Temperature()
        {
            return "{Temperature: " + weatherData.Temperature() + "}";
        }

        public string WindSpeed()
        {
            return "{WindSpeed: " + weatherData.WindSpeed() + "}";
        }
    }

    public static class RandomExtensions
    {
        public static double NextDouble(this Random rnd, double min, double max, int decPlaces = 2)
        {
            double val = min + (max - min) * rnd.NextDouble();
            return Math.Round(val, decPlaces);
        }
    }

    #endregion


    #region Naloga 15.2.2

    public interface IBook
    {
        void ShowContent();
    }

    public class RealBook : IBook
    {
        public string Title { get; }

        public int Pages { get; }

        public RealBook(string title, int pgs)
        {
            this.Title = title;
            this.Pages = pgs;
            LoadContent();
        }

        private void LoadContent()
        {
            Console.WriteLine($"Nalagamo vsebino za knjigo z naslovom {this.Title}...");
            Thread.Sleep(this.Pages);
        }

        public void ShowContent()
        {
            Console.WriteLine($"Odprli smo knjigo {this.Title} in beremo!");
        }
    }

    public class BookProxy : IBook
    {
        private RealBook? book;
        private string title;
        private int pages;

        public BookProxy(string title, int pgs)
        {
            this.title = title;
            this.pages = pgs;
        }

        public void ShowContent()
        {
            if (book == null)
            {
                book = new RealBook(this.title, this.pages);
            }
            book.ShowContent();
        }
    }

    #endregion


    #region Naloga 15.2.3

    public interface IEmployeesData
    {
        public double GetSalary(string davcnaStevilka);
    }

    public class EmployeesData : IEmployeesData
    {
        public double GetSalary(string davcnaStevilka)
        {
            return 1500.00;
        }
    }

    public class EmployeesDataProxy : IEmployeesData
    {
        private EmployeeRole role;

        private EmployeesData employeesData;

        public EmployeesDataProxy(EmployeeRole role)
        {
            this.role = role;
            this.employeesData = new EmployeesData();
        }

        public double GetSalary(string davcnaStevilka)
        {
            if (role == EmployeeRole.Administrator)
            {
                return employeesData.GetSalary(davcnaStevilka);
            }
            else
            {
                Console.WriteLine($"Uporabnik z vlogo {role} nima dostopa do plače.");
                return 0;
            }
        }
    }

    public enum EmployeeRole
    {
        Guest,
        User,
        Administrator
    }

    #endregion
}
