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

                case Tutorial.Tutorial_02:
                    {
                        switch (InterfaceFunctions.ChooseOption<Tutorials_02.Exercises>())
                        {
                            case Tutorials_02.Exercises.Exercise_574:
                                {
                                    Tutorials_02.Exercise_574();
                                }
                                break;
                            case Tutorials_02.Exercises.Exercise_575:
                                {
                                    Tutorials_02.Exercise_575();
                                }
                                break;
                        }
                    }
                    break;

                case Tutorial.Tutorial_03:
                    {
                        switch (InterfaceFunctions.ChooseOption<Tutorials_03.Exercises>())
                        {
                            case Tutorials_03.Exercises.Exercise_621:
                                {
                                    Tutorials_03.Exercise_621();
                                }
                                break;
                            case Tutorials_03.Exercises.Exercise_624:
                                {
                                    Tutorials_03.Exercise_624();
                                }
                                break;
                        }
                    }
                    break;

                case Tutorial.Tutorial_04:
                    {
                        switch (InterfaceFunctions.ChooseOption<Tutorials_04.Exercises>())
                        {
                            case Tutorials_04.Exercises.Exercise_721:
                                {
                                    Tutorials_04.Exercise_721();
                                }
                                break;
                            case Tutorials_04.Exercises.Exercise_722:
                                {
                                    Tutorials_04.Exercise_722();
                                }
                                break;
                            case Tutorials_04.Exercises.Exercise_725:
                                {
                                    Tutorials_04.Exercise_725();
                                }
                                break;
                        }
                    }
                    break;

                case Tutorial.Tutorial_05:
                    {
                        switch (InterfaceFunctions.ChooseOption<Tutorials_05.Exercises>())
                        {
                            case Tutorials_05.Exercises.Exercise_921:
                                {
                                    Tutorials_05.Exercise_921();
                                }
                                break;
                        }
                    }
                    break;
            }            
        }
    }
}
