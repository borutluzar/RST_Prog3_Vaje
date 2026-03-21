using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public class Tutorials_02
    {
        public enum Exercises
        {
            Exercise_574 = 1,
            Exercise_575 = 2
        }

        /// <summary>
        /// Postali ste del razvojne ekipe večjega sistema. 
        /// Vaša prva naloga je, da pripravite podrazred NadzornaKomisija razreda Komisija, 
        /// ki bo vseboval funkcijo PreveriClana. 
        /// Funkcija z enakim imenom že obstaja v nadrazredu, 
        /// vendar zaradi politike podjetja, tega razreda trenutno ne morete spreminjati, 
        /// funkcija pa tudi ni označena kot virtual, se pravi, da je ne morete povoziti.
        /// Ali lahko funkcijo z enakim imenom sploh dodate v podrazred? 
        /// Če da, kako in kako se razlikuje njeno obnašanje glede na funkcije, ki jih povozimo z override?
        /// </summary>
        public static void Exercise_574()
        {
            Komisija i1 = new NadzornaKomisija(new DateOnly(2026, 3, 20));

            i1.PreveriClana(1);
            i1.PreveriClana2(1);
        }

        /// <summary>
        /// Definirajte razreda Menu in Jed. 
        /// Menu naj predstavlja dnevni menu v restavraciji (glede na dan), 
        /// ki ima kot lastnost tudi seznam jedi. 
        /// Posamezna jed ima lastnosti naziv in cena.
        /// Za razred Jed naredite podrazred Sladica, ki bo imel dodatno lastnost Kalorije. 
        /// V razredih Jed in Sladica povozite funkcijo ToString, da bo ustrezno vračala vse lastnosti instanc.
        /// Funkcijo ToString povozite tudi v razredu Menu. 
        /// Vrne naj niz z dnevom in vsemi jedmi, ki so na menuju, med seboj pa naj bodo ločene s prazno vrstico. 
        /// V razredu Menu napišite še funkcijo, ki bo izpisala skupno ceno menuja. 
        /// Funkcija naj ima vhodni parameter tipa bool, ki bo določal, 
        /// ali želite ob ceni plačati še 10% napitnine ali ne. 
        /// Če je vrednost parametra true, naj se skupna cena primerno izračuna. 
        /// Za vsaj dva dni v tednu pripravite instanci razreda Menu, 
        /// ki bosta imeli na seznamu jedi vsaj po tri jedi, od tega vsak natanko eno jed tipa Sladica. 
        /// Na koncu oba menuja tudi izpišite.
        /// </summary>
        public static void Exercise_575()
        {
            Jed spageti = new Jed("Špageti", 6.00);

            Jed solata = new Jed("Solata", 4.50);

            Sladica kremsnite = new Sladica("Kremšnite", 5.50, 600);

            Menu meni = new Menu("Petek");

            meni.SeznamJedi.Add(spageti);
            meni.SeznamJedi.Add(solata);
            meni.SeznamJedi.Add(kremsnite);

            Console.WriteLine(meni);

            Console.Write($"Cena menija v {meni.Dan} je: {meni.CenaMenija(true)}");
        }
    }


    public class Komisija
    {
        public DateOnly DatumUstanovitve { get; }

        public Komisija(DateOnly du)
        {
            DatumUstanovitve = du;
        }

        public bool PreveriClana(int idClana)
        {
            Console.WriteLine("PreveriClana (Komisija)");
            return false;
        }

        public virtual bool PreveriClana2(int idClana)
        {
            Console.WriteLine("PreveriClana2 (Komisija)");
            return false;
        }
    }

    public class NadzornaKomisija : Komisija
    {
        public NadzornaKomisija(DateOnly du) : base(du) { }

        public bool PreveriClana(int idClana)
        {
            Console.WriteLine("PreveriClana (NadzornaKomisija)");
            return false;
        }

        public override bool PreveriClana2(int idClana)
        {
            Console.WriteLine("PreveriClana2 (NadzornaKomisija)");
            return false;
        }
    }

    public class Menu
    {
        public List<Jed> SeznamJedi { get; } = new List<Jed>();

        public string Dan { get; }

        public Menu(string dan)
        {
            Dan = dan;
        }

        public override string ToString()
        {
            string output = $"{Dan} \n";

            foreach (Jed jed in SeznamJedi)
            {
                output += jed.ToString() + $"\n";
            }

            return output;
        }

        public double CenaMenija(bool dajNapitnino)
        {
            double vsotaMenija = 0;

            foreach (Jed jed in SeznamJedi)
            {
                vsotaMenija += jed.Cena;
            }

            if (dajNapitnino)
            {
                vsotaMenija = vsotaMenija * 1.10;
            }

            return vsotaMenija;
        }
    }

    public class Jed
    {
        public string Naziv { get; }
        public double Cena { get; }

        public Jed(string naziv, double cena)
        {
            Naziv = naziv;
            Cena = cena;
        }

        public override string ToString()
        {
            return $"{Naziv} - {Cena} EUR";
        }
    }

    public class Sladica : Jed
    {
        public int Kalorije { get; set; }

        public Sladica(string naziv, double cena, int kalorije) : base(naziv, cena)
        {
            Kalorije = kalorije;
        }

        public Sladica(string naziv, double cena) : base(naziv, cena) { }

        public override string ToString()
        {
            return base.ToString() + $" ({Kalorije} kcal)";
        }
    }
}
