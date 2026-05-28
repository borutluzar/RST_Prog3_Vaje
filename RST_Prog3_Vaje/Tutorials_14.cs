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
    public class Tutorials_14
    {
        public enum Exercises
        {
            Exercise_1724 = 1,
        }

        /// <summary>
        /// Navodila v zapiskih.
        /// </summary>
        public static void Exercise_1724()
        {
            HousingStrategy housingStrategy = new();
            //Console.WriteLine( $"Osnovni obrok: {housingStrategy.CalculateMonthlyInstallment(20000.0m, 6):0.00}");
            InsuranceDecorator inusredLoan = new InsuranceDecorator(housingStrategy, InsuranceType.Basic, 20.0m);
            //Console.WriteLine($"{inusredLoan.LoanType}: {inusredLoan.CalculateMonthlyInstallment(20000.0m, 6):0.00}");
            InsuranceDecorator inusredLoanWithCatastrophe = new InsuranceDecorator(inusredLoan, InsuranceType.NaturalCatastrophe, 5.0m);
            //Console.WriteLine($"{inusredLoanWithCatastrophe.LoanType}: {inusredLoanWithCatastrophe.CalculateMonthlyInstallment(20000.0m, 6):0.00}");
            PersonalTreatmentDecorator personal2 = new PersonalTreatmentDecorator(inusredLoanWithCatastrophe, 100.0m);
            Console.WriteLine($"{personal2.LoanType}: {personal2.CalculateMonthlyInstallment(20000.0m, 6):0.00}");
            Loan loan1 = new Loan(20000.0m, 6, personal2);

            ConsumerStrategy consumerStrategy = new();
            PersonalTreatmentDecorator personal = new PersonalTreatmentDecorator(consumerStrategy, 100.0m);
            Console.WriteLine($"{personal.LoanType}: {personal.CalculateMonthlyInstallment(20000.0m, 6):0.00}");
            Loan loan2 = new Loan(20000.0m, 6, personal);

            CentralRegistry registry = CentralRegistry.Init();
            registry.AddLoan(loan1);
            registry.AddLoan(loan2);
        }
    }


    #region Naloga 17.2.4

    public class Loan
    {
        public decimal Amount { get; set; }
        public int Months { get; set; }
        ILoanStrategy? Strategy { get; set; }

        public Loan(decimal amount, int months, ILoanStrategy strategy)
        {
            Amount = amount;
            Months = months;
            Strategy = strategy;
        }

        public decimal ProcessLoan()
        {
            decimal? tmp = Strategy?.CalculateMonthlyInstallment(Amount, Months);
            if (tmp == null)
            {
                throw new Exception("Null");
            }
            Console.WriteLine($"Monthly installment: {tmp:0.00}");

            return tmp.Value;
        }
    }

    public enum InsuranceType
    {
        Basic,
        Death,
        Injury,
        NaturalCatastrophe
    }

    public class InsuranceDecorator : ILoanStrategy
    {
        private ILoanStrategy strategy;
        private decimal insuranceCost = 20.0m;
        private InsuranceType insuranceType;

        public InsuranceDecorator(ILoanStrategy strategy, InsuranceType insuranceType, decimal insuranceCost)
        {
            this.strategy = strategy;
            this.insuranceType = insuranceType;
            this.insuranceCost = insuranceCost;
        }

        public string LoanType
        {
            get
            {
                return this.strategy.LoanType + ", z zavarovanjem tipa " + insuranceType.ToString();
            }
        }

        public decimal CalculateMonthlyInstallment(decimal amount, int months)
        {
            decimal temp = this.strategy.CalculateMonthlyInstallment(amount, months);
            return temp + insuranceCost;
        }
    }

    public class PersonalTreatmentDecorator : ILoanStrategy
    {
        private ILoanStrategy strategy;
        private decimal personalReduction = 20.0m;

        public PersonalTreatmentDecorator(ILoanStrategy strategy, decimal personalReduction)
        {
            this.strategy = strategy;
            this.personalReduction = personalReduction;
        }

        public string LoanType
        {
            get
            {
                return this.strategy.LoanType + ", z osebno obravnavo";
            }
        }

        public decimal CalculateMonthlyInstallment(decimal amount, int months)
        {
            decimal temp = this.strategy.CalculateMonthlyInstallment(amount, months);
            return temp - personalReduction;
        }
    }

    class HousingStrategy : ILoanStrategy
    {
        public string LoanType { get; } = $"Hišni kredit({interestRate})";

        private const decimal interestRate = 2.8m;

        public decimal CalculateMonthlyInstallment(decimal amount, int months)
        {
            return (amount * (1 + (interestRate / 100))) / months;
        }
    }

    class ConsumerStrategy : ILoanStrategy
    {
        public string LoanType { get; } = $"Potrošniški kredit({interestRate})";

        private const decimal interestRate = 2.7m;

        public decimal CalculateMonthlyInstallment(decimal amount, int months)
        {
            return (amount * (1 + (interestRate / 100))) / months;
        }
    }

    public interface ILoanStrategy
    {
        public string LoanType { get; }

        public decimal CalculateMonthlyInstallment(decimal amount, int months);
    }

    public sealed class CentralRegistry
    {
        static CentralRegistry? centralRegistry;

        public decimal FinancialExposure { get; private set; }

        private CentralRegistry() { }

        public static CentralRegistry Init()
        {
            if (centralRegistry == null)
            {
                centralRegistry = new CentralRegistry();
            }
            return centralRegistry;
        }

        public void AddLoan(Loan loan)
        {
            decimal tmp = loan.ProcessLoan();
            FinancialExposure += tmp;
            Console.WriteLine($"Skupna izpostavljenost banke je {FinancialExposure:0.00}");
        }
    }

    #endregion
}
