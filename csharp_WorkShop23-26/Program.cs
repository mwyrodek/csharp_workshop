using System;
using NUnit.Framework;

namespace csharp_WorkShop23_26
{
    class Program
    {
        static void Main(string[] args)
        {
            //This is Your Starting point
            // Build it  (Ctrl+Shift+B)
            //and run it (F5)

            Console.WriteLine("This is Program To show you how to run programs");
            var isNumber = false;
            var result = 0;
            do
            {
                Console.WriteLine("Enter any digit");
                string keyInfo = Console.ReadKey().KeyChar.ToString();
                isNumber = int.TryParse(keyInfo, out result);
                if (!isNumber)
                {
                    Console.WriteLine("\nThis is not digit! One more time!");
                }


            } while (!isNumber);

            Console.WriteLine($"\nyour number is {result}");
           
            
            Assert.AreEqual(4,result, "That is just to force you to fix build");
        }
    }
}
