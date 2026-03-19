using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace RST_Prog3_Vaje
{
    public class Tutorials_01
    {
        /// <summary>
        /// Pripravite razred Index z ustreznimi lastnostmi, predvsem naj vsebuje seznam predmetov. 
        /// Posamezen predmet naj bo instanca razreda Subject, ki ima lastnosti Name in Grade.
        /// Lastnost Grade naj dovoli samo vrednosti med 5 in 10. 
        /// Če poskušate nastaviti drugo vrednost, 
        /// naj se samodejno popravi na najbližjo dovoljeno vrednost.
        /// V razred Indeks dodajte še lastnost PovprecnaOcena kot samo za branje, 
        /// ki izračuna povprečje ocen predmetov.
        /// </summary>
        public static void Exercise_324()
        {
            // Kreiramo nov indeks:
            Index index = new Index(20, StudyProgram.RST);
            index.Year = 2;

            Subject prog3 = new Subject(1)
            {
                Name = "Programiranje 3",
                Grade = 10,
                ECTS = 6,
                DateCompleted = new DateOnly(2026, 6, 4)
            };
            Subject baze = new Subject(2)
            {
                Name = "Baze in modeliranje podatkov",
                Grade = 10,
                ECTS = 6,
                DateCompleted = new DateOnly(2026, 6, 10)
            };

            index.Subjects.Add(prog3);
            index.Subjects.Add(baze);

            Console.WriteLine($"{index}");

            Console.WriteLine($"Povprečje ocen študenta {index.StudentID} je {index.AverageGrade:0.00}");
            Console.Read();
        }
    }


    /// <summary>
    /// Razred, ki predstavlja študentski indeks.
    /// </summary>
    public class Index
    {
        public Index(int id, StudyProgram program)
        {
            this.StudentID = id;
            this.Program = program;
        }

        public int StudentID { get; }

        public StudyProgram Program { get; }

        private int year;
        
        /// <summary>
        /// Lastnost implementiramo v celoti, da lahko dodamo validacijo vrednosti ob nastavljanju
        /// </summary>
        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1 || value > 3)
                {
                    throw new ArgumentException("Napačno leto študija!");
                }
                year = value;
            }
        }

        public List<Subject> Subjects { get; } = new List<Subject>();

        public double AverageGrade
        {
            get
            {
                return Subjects.Average(x => x.Grade);
            }
        }

        public override string ToString()
        {
            string output = $"{this.StudentID} ({this.Program}): ";
            foreach (var subject in this.Subjects)
            {
                output += $"\n\t{subject}";
            }
            return output;
        }
    }

    /// <summary>
    /// Enumeracija s študijskimi programi
    /// </summary>
    public enum StudyProgram
    {
        RST = 1,
        ISD = 2,
        RVRR = 3,
        iRST = 4,
        iISD = 5
    }

    /// <summary>
    /// Razred, ki predstavlja posamezen predmet
    /// </summary>
    public class Subject
    {
        public Subject(int id)
        {
            this.ID = id;
        }

        public int ID { get; }
        public string Name { get; set; }
        public int ECTS { get; set; }

        private int grade;
        public int Grade 
        {
            get { return grade; }
            set
            {
                if (value < 5)
                {
                    grade = 5;
                }
                else if(value > 10)
                {
                    grade = 10;
                }
                else
                {
                    grade = value;
                }                
            } 
        }
        public DateOnly DateCompleted { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Grade} [{ECTS}], opravljeno: {DateCompleted:dd.MM.yyyy}";
        }
    }
}
