using System;
using System.Collections.Generic;

namespace PinTools
{
    class Program
    {
        static void Main(string[] args)
        {
            PinGenerator pinGenerator = new PinGenerator();

            try
            {
                for (int i = 0; i < 100000; i++)
                {
                    string pin = pinGenerator.GeneratePin();
                    Console.Write(pin + ", ");
                }
#if DEBUG
                Console.ReadKey();
#endif
                Environment.Exit(0);

            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error: " + e.Message);
                Environment.Exit(-1);
            }

        }
    }

}
