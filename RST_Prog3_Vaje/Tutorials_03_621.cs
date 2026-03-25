using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RST_Prog3_Vaje
{
    public abstract class Article
    {
        public Article(int id)
        {
            this.Id = id;
        }

        public double Price { get; set; }
        public int Id { get; }
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string Name { get; set; } = string.Empty;

        public abstract double ComputeDeliveryCost(double basicCost);
        public abstract string GetInstructions();
        public override string ToString()
        {
            return $"{this.Name} ({this.Price}eur)\n\t{this.Description}";
        }
    }

    public abstract class Clothing : Article
    {
        public Clothing(int id, ClothesSize cs, string material) : base(id)
        {
            this.Size = cs;
            this.Material = material;
        }

        public ClothesSize Size { get; }
        public string Material { get; }

    }
    public enum ClothesSize : byte
    {
        XS,
        S,
        M,
        L,
        XL,
        XXL
    }

    public abstract class Footwear : Article
    {
        public Footwear(int id, ShoeSizeEU ss, string st) : base(id)
        {
            this.ShoeSize = ss;
            this.SoleType = st;
        }

        public ShoeSizeEU ShoeSize { get; }
        public string SoleType { get; }
    }
    public enum ShoeSizeEU : byte
    {
        s38,
        s38_5,
        s39,
        s39_5,
        s40,
        s41
    }

    public abstract class Accessory : Article
    {
        public Accessory(int id, double weight, bool rb) : base(id)
        {
            this.Weight = weight;
            this.RequiresBattery = rb;
        }

        public double Weight { get; }
        public bool RequiresBattery { get; }
    }

    public class Pants : Clothing
    {
        public Pants(int id, ClothesSize cs, string material, string fitting) : base(id, cs, material)
        {
            this.Fitting = fitting;
        }

        public string Fitting { get; }

        public override double ComputeDeliveryCost(double basicCost)
        {
            return basicCost * 10;
        }

        public override string GetInstructions()
        {
            return "Ne peremo v vroci vodi.";
        }

        public override string ToString()
        {
            return base.ToString() + $"\nKroj: {this.Fitting}";
        }
    }

    public class PhoneHolder : Accessory
    {
        public PhoneHolder(int id, double weight, bool rb) : base(id, weight, rb) { }

        public override double ComputeDeliveryCost(double basicCost)
        {
            return basicCost * this.Weight;
        }

        public override string GetInstructions()
        {
            return "Maximum phone weight 2kg";
        }

        public override string ToString()
        {
            return base.ToString() + $"\nTeža: {this.Weight}";
        }
    }

}
