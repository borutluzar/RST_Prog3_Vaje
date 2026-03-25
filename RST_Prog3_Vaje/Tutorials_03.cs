using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public class Tutorials_03
    {
        public enum Exercises
        {
            Exercise_621 = 1,
            Exercise_624 = 2
        }

        /// <summary>
        /// Pripravite abstraktni razred Article, ki naj ima lastnosti Name, Price, Stock 
        /// in dve abstraktni funkciji GetInstructions, ComputeDeliveryCosts. 
        /// Prepišite tudi funkcijo ToString. 
        /// 
        /// Iz razreda Article izpeljite tri abstraktne podrazrede: 
        /// Clothing (lastnosti Size in Material),  
        /// Footwear (lastnosti ShoeSize in SoleType) in 
        /// Accessory (lastnosti Weight in RequiresBatteries). 
        /// 
        /// Iz vsakega vmesnega razreda izpeljite vsaj dva konkretna razreda 
        /// (npr. TShirt in Jacket iz oblačil, Sneakers iz obutve, Smartwatch iz pripomočkov). 
        /// 
        /// Za vsakega: 
        /// - Implementirajte obe abstraktni funkciji z logiko, primerno za ta izdelek.
        /// - V vsakem povozite funkcijo ToString. 
        /// 
        /// Na koncu v funkciji Main (oziroma v funkciji, kjer izvajate nalogo) 
        /// ustvarite seznam artiklov, vanj dodajte po vsaj eno instanco vsakega od neabstraktnih razredov. 
        /// Za vsako instanco pokličite funkcije ToString, GetInstructions in ComputeDeliveryCosts.
        /// </summary>
        public static void Exercise_621()
        {
            Pants hlace = new(1, ClothesSize.L, "Jeans", "regular") { Name = "Hlače" };
            PhoneHolder drzalo = new(2, 0.3, false) { Name = "Držalo za mobilni telefon" };

            List<Article> lstArticles = new List<Article>() { hlace, drzalo };

            foreach (Article article in lstArticles)
            {
                Console.WriteLine(article.ToString());
                Console.WriteLine($"Stroski dostave so: {article.ComputeDeliveryCost(0.5)}");
                Console.WriteLine($"{article.GetInstructions()}");
            }
        }

        /// <summary>
        /// Domača osnovna šola vam je naročila pripravo programa, 
        /// ki bo znal upravljati s preprostimi geometrijskimi operacijami. 
        /// Na primer, za dane like (trikotnik, kvadrat, pravokotnik, ...) 
        /// in njihove velikosti (npr.dolžine stranic), bi moral program izračunati 
        /// ploščino, obseg in druge lastnosti. 
        /// Vaša naloga je, da pripravite modele (abstraktne razrede, podrazrede), 
        /// ki bodo predvideli potrebne funkcije in lastnosti.
        /// </summary>
        public static void Exercise_624()
        {
            Circle krog = new Circle(2.0);
            Rectangle pravokotnik = new Rectangle(3.0, 4.0);
            Square kvadrat = new Square(5.0);

            List<Shape> shapes = new List<Shape>() { krog, pravokotnik, kvadrat };
            foreach (Shape shape in shapes)
            {
                Console.WriteLine($"Lik {shape.Name} ima ploščino {shape.Area():0.00} in obseg {shape.Perimeter():0.00}");
            }
        }
    }


}
