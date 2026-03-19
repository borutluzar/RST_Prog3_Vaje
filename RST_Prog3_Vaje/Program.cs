using RST2_Programiranje_Vaje_25_26;
using static System.Collections.Specialized.BitVector32;

namespace RST_Prog3_Vaje
{
    internal class Program
    {
        enum Tutorial
        {
            Tutorial_01 = 1,  // 16. 3. 2026
            Tutorial_02 = 2,  // 20. 3. 2026
            Tutorial_03 = 3,  // 25. 3. 2026
            Tutorial_04 = 4,  //  7. 4. 2026
            Tutorial_05 = 5,  //  9. 4. 2026
            Tutorial_06 = 6,  // 10. 4. 2026
            Tutorial_07 = 7,  // 13. 4. 2026
            Tutorial_08 = 8,  // 23. 4. 2026
            Tutorial_09 = 9,  //  6. 5. 2026
            Tutorial_10 = 10, // 13. 5. 2026
            Tutorial_11 = 11, // 15. 5. 2026
            Tutorial_12 = 12, // 20. 5. 2026
            Tutorial_13 = 13, // 21. 5. 2026
            Tutorial_14 = 14, // 26. 5. 2026
            Tutorial_15 = 15, // 28. 5. 2026
        }

        static void Main(string[] args)
        {
            switch (InterfaceFunctions.ChooseOption<Tutorial>())
            {
                case Tutorial.Tutorial_01:
                    {
                        Tutorials_01.Exercise_324();
                    }
                    break;
            }
        }
    }
}
