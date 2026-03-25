using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public abstract class Shape
    {
        public string Name { get; set; } = string.Empty;
        
        public abstract double Perimeter();
        public abstract double Area();
    }

    public class Circle : Shape
    {
        public double Radius { get; }

        public Circle(double r)
        {
            this.Name = "Krog";
            this.Radius = r;
        }

        public override double Perimeter()
        {
            return 2 * Math.PI * Radius;
        }

        public override double Area()
        {
            return Math.PI * Math.Pow(Radius, 2);
        }
    }

    public abstract class NGon : Shape
    {
        public int N { get; }

        public NGon(int n)
        {
            this.Name = "n-Gon";
            this.N = n;
        }

    }

    public class Rectangle : NGon
    {
        public Rectangle(double a, double b) : base(4)
        {
            this.Name = "Rectangle";
            this.A = a;
            this.B = b;
        }

        public double A { get; }
        public double B { get; }

        public override double Area()
        {
            return A * B;
        }

        public override double Perimeter()
        {
            return 2 * A + 2 * B;
        }
    }

    public class Square : Rectangle
    {
        public Square(double a) : base(a, a) 
        { 
            this.Name = "Square";
        }
    }
}
