
using RestaurantMenu;
using System.Xml.Linq;

namespace RestaurantMenu
{
    public enum CondimentsList
    {
        Mayonaise = 1,
        Ketchup,
        Mustard
    }

    abstract class Food
    {
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool DoubleSize { get; set; } = false;
        public CondimentsList[] Condiments { get; set; }

        public Food(double price, int quantity, bool doubleSize)
        {
            Price = price;
            Quantity = quantity;
            DoubleSize = doubleSize;
        }

        public Food(double price, int quantity, bool doubleSize, CondimentsList[] condiments) : this(price, quantity, doubleSize)
        {
            Condiments = condiments;
        }


        public static int ChooseQuantity(string name)
        {
            bool valid = false;
            int quantity = 0;
            while (!valid)
            {
                Console.Write($"\nHow many {name}s would you like?: ");
                quantity = Program.ReadUserInput("QuantityMenu");
                if (quantity <= 0) Console.WriteLine($"You can't buy zero\n");
                else valid = true;
            }

            if (quantity == 1) Console.WriteLine($"You chose to buy a single {name}\n");
            else Console.WriteLine($"You chose to buy {quantity} {name}s\n");
            return quantity;
        }

        public static double ChooseSize(out bool isDouble, double simplePrice, double doublePrice)
        {
            isDouble = false;

            Console.WriteLine($"1. Simple: {simplePrice.ToString("C")}");
            Console.WriteLine($"2. Double: {doublePrice.ToString("C")}");
            Console.Write("Choose size: ");

            int userInput = Program.ReadUserInput("SizeMenu");

            switch (userInput)
            {
                case 1: Console.WriteLine("\nYou've chosen simple size\n"); ; return simplePrice;

                case 2: isDouble = true; Console.WriteLine("\nYou've chosen double size\n"); return doublePrice;

                default: return simplePrice;
            }

        }

        public static CondimentsList[] SelectCondiments()
        {
            bool hasMayo = false;
            bool hasKet = false;
            bool hasMus = false;
            bool isChoosing = true;
            do
            {
                string Mayo = hasMayo == false ? "Not added" : "Added";
                string Ket = hasKet == false ? "Not added" : "Added";
                string Mus = hasMus == false ? "Not added" : "Added";

                Console.WriteLine($"1. {CondimentsList.Mayonaise} = {Mayo}");
                Console.WriteLine($"2. {CondimentsList.Ketchup} = {Ket}");
                Console.WriteLine($"3. {CondimentsList.Mustard} = {Mus}");
                Console.WriteLine($"4. Finish");

                Console.Write("\nChoose your condiments: ");
                int userInput = Program.ReadUserInput("CondimentsMenu");

                switch (userInput)
                {
                    case 1: hasMayo = hasMayo == false ? true : false; Console.Clear(); break;
                    case 2: hasKet = hasKet == false ? true : false; Console.Clear(); break;
                    case 3: hasMus = hasMus == false ? true : false; Console.Clear(); break;
                    case 4: isChoosing = false; break;
                    default: Console.WriteLine("Invalid option"); break;
                }

            } while (isChoosing);

            int sum = SumCondiments(hasMayo, hasKet, hasMus);

            CondimentsList[] list = new CondimentsList[sum];

            for (int i = 0; i < list.Length; i++)
            {
                if (hasMayo)
                {
                    list[i] = CondimentsList.Mayonaise;
                    hasMayo = false;
                }
                else if (hasKet)
                {
                    list[i] = CondimentsList.Ketchup;
                    hasKet = false;
                }
                else if (hasMus)
                {
                    list[i] = CondimentsList.Mustard;
                    hasMus = false;
                }

            }
            return list;
        }
        private static int SumCondiments(bool hasMayo, bool hasKet, bool hasMus)
        {
            int sum = 0;
            if (hasMayo == true)
                sum += 1;
            if (hasKet == true)
                sum += 1;
            if (hasMus == true)
                sum += 1;

            return sum;
        }

        public static string UpperFirst(string name)
        {
            char H = name[0];
            H = char.ToUpper(H);
            name = name.Remove(0, 1);
            return H + name;
        }

        public static string DisplayOrder(bool isDouble, string name, int quantity, double price, string output)
        {
            if(isDouble)
                return $"\n--- Your order ---\n\nItem: Double {name}\nQuantity: {quantity}\nPrice: {price.ToString("C")}\nCondiments: {output}";
            else
                return $"\n--- Your order ---\n\nItem: {name}\nQuantity: {quantity}\nPrice: {price.ToString("C")}\nCondiments: {output}";
        }
        public static string DisplayOrder(bool isDouble, string name, int quantity, double price)
        {
            if(isDouble)
                return $"\n--- Your order ---\n\nItem: Double {name}\nQuantity: {quantity}\nPrice: {price.ToString("C")}";
            else
                return $"\n--- Your order ---\n\nItem: {name}\nQuantity: {quantity}\nPrice: {price.ToString("C")}";

        }

    }

    class Burger : Food
    {
        private static string name = "burger";
        private static double simplePrice = 6.99;
        private static double doublePrice = 11.99;


        public Burger(double price, int quantity, bool doubleSize, CondimentsList[] condiments) : base(price, quantity, doubleSize, condiments)
        {
        }

        public override string ToString()
        {
            string output = "";
            Price *= Quantity;
            foreach (CondimentsList c in Condiments)
            {
                output += $"{c} | ";
            }
            return DisplayOrder(DoubleSize, name, Quantity, Price, output);
        }

        public static Burger Create()
        {
            int quantity = ChooseQuantity(name);
            double price = ChooseSize(out bool isDouble, simplePrice, doublePrice);
            CondimentsList[] condiments = SelectCondiments();
            name = UpperFirst(name);
            return new Burger(price, quantity, isDouble, condiments);
        }




    }

    class Hotdog : Food
    {
        private static string name = "hotdog";
        private static double simplePrice = 2.99;
        private static double doublePrice = 4.99;
        public Hotdog(double price, int quantity, bool doubleSize, CondimentsList[] condiments) : base(price, quantity, doubleSize, condiments)
        {
        }

        public static Hotdog Create()
        {
            int quantity = ChooseQuantity(name);
            double price = ChooseSize(out bool isDouble, simplePrice, doublePrice);
            CondimentsList[] condiments = SelectCondiments();

            return new Hotdog(price, quantity, isDouble, condiments);
        }

        public override string ToString()
        {
            string output = "";
            Price *= Quantity;
            foreach (CondimentsList c in Condiments)
            {
                output += $"{c} | ";
            }
            name = UpperFirst(name);
            return DisplayOrder(DoubleSize, name, Quantity, Price, output);
        }
    }

    class Pizza : Food
    {
        private static string name = "pizza";
        private static double simplePrice = 3.99;
        private static double doublePrice = 7.99;

        public Pizza(double price, int quantity, bool doubleSize) : base(price, quantity, doubleSize)
        {

        }

        public static Pizza Create()
        {
            int quantity = ChooseQuantity(name);
            double price = ChooseSize(out bool isDouble, simplePrice, doublePrice);

            return new Pizza(price, quantity, isDouble);
        }

        public override string ToString()
        {
            Price *= Quantity;
            name = UpperFirst(name);
            return DisplayOrder( DoubleSize, name, Quantity, Price);
        }

    }


    class Program
    {
        static void Main()
        {
            PrintMenu();

            int userInput = ReadUserInput("Menu");

            ProcessInput(userInput);

            Console.ReadLine();
        }

        static void ProcessInput(int userInput)
        {
            bool valid = false;

            while (!valid)
            {
                switch (userInput)
                {
                    case 1: BurgerChosen(); valid = true; break;
                    case 2: HotdogChosen(); valid = true; break;
                    case 3: PizzaChosen(); valid = true; break;
                    default: Console.Clear(); Console.WriteLine("Invalid option\n"); PrintMenu(); userInput = ReadUserInput("Menu"); break;
                }
            }

        }

        static void PrintMenu()
        {
            Console.WriteLine("--- MENU ---\n");
            Console.WriteLine("1. Burger");
            Console.WriteLine("2. Hotdog");
            Console.WriteLine("3. Pizza");
            Console.Write("\nChoose a food: ");
        }

        static void BurgerChosen()
        {
            Burger burger = Burger.Create();
            Console.WriteLine(burger);
        }

        static void HotdogChosen()
        {
            Hotdog hotdog = Hotdog.Create();
            Console.WriteLine(hotdog);
        }
        static void PizzaChosen()
        {
            Pizza pizza = Pizza.Create();
            Console.WriteLine(pizza);
        }

        public static int ReadUserInput(string context)
        {
            int input = 0;
            bool valid = false;
            while (!valid && input == 0)
            {
                try
                {
                    input = Convert.ToInt16(Console.ReadLine());
                    valid = true;
                }
                catch (FormatException)
                {
                    PrintError("Please, enter only numbers", context);
                }
                catch (OverflowException)
                {
                    PrintError("Number too big!", context);
                }
                catch (Exception e)
                {
                    PrintError(e.Message, context);
                }
            }
            return input;
        }

        static void PrintError(string text, string context)
        {
            switch (context)
            {
                case "Menu": Console.Clear(); Console.WriteLine(text + "\n"); PrintMenu(); break;
                case "SizeMenu": Console.WriteLine(text + "\n"); break;
                default: Console.WriteLine(text + "\n"); break;
            }



        }
    }

}


