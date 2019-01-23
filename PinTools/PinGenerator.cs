using PinTools.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace PinTools
{
    /// <summary>
    /// Generates a 4-digit PIN Number.
    /// </summary>
    /// <remarks>The generator will generate a 4-digit PIN number.
    /// 
    /// PIN Numbers will not be repeated until all other numbers have
    /// been exhausted. This is achieved by persisting previously generated
    /// PIN numbers to JSON file called 'used-numbers.json' in the current directory.
    /// It will also ignore any PIN numbers defined in the 'barred-numbers.json' file in the current directory. 
    /// The format of these files is as per the example below:
    /// [ '0000', '1234', '1111', '9999' ]
    /// </remarks>

    public class PinGenerator
    {
        // private constants
        private const int MaxNumber = 9999;
        private const string BarredNumberStoreFilename = "barred-numbers.json";
        private const string UsedNumberStoreFilename = "used-numbers.json";

        // In memory list of all disallowed numbers      
        private readonly List<string> _barredNumbers = new List<string>() { "1111","1234", "9999" };

        // In memory list of previously used numbers
        private readonly List<string> _usedNumbers = new List<string>();   
        
        private readonly JsonDataStore _usedNumberStore;

        // Global static reference to Random to avoid duplicate seeding if called 
        // many times within a short timespan
        private static readonly Random generator = new Random();            

        // ctor
        public PinGenerator()
        {
            try
            {
               
                // Load the list of previously generated numbers from the local data store
                _usedNumberStore = new JsonDataStore(UsedNumberStoreFilename);
                _usedNumbers = _usedNumberStore.LoadList();

#if DEBUG
                if (_usedNumbers != null)
                {
                    string[] unarr = _usedNumbers?.ToArray();
                    Console.WriteLine("Used Numbers: " + String.Join(", ", unarr) + "\n");
                }
#endif
            }
            catch (FileNotFoundException)
            {
                // We can trap & ignore this error as on first run it's possible 
                // we won't have any used numbers defined
            }

        }

        /// <summary>
        /// Generates a new 4-digit PIN number.
        /// </summary>
        /// <returns>The generated PIN number</returns>
        public string GeneratePin()
        {
            string pin;
            int retryAttempts = 0;

            // Generate a candidate PIN number, check that it is valid (i.e. not on the barred list)
            // and hasn't been generated previously. If it has, discard it and try again
            do
            {

                pin = generator.Next(0, MaxNumber).ToString("D4");

                // The following code handles the situation where
                // the list of previously used pin numbers contains all possible combinations
                // of numbers and so no pin can be generated. In this eventuality we
                // clear the list and start again.
                retryAttempts++;
                if (retryAttempts > MaxNumber)
                {
                    ClearUsedNumbers();
                }

            }
            while (!IsValidNumber(pin) && (IsPreviouslyUsed(pin)));

            // Add the pin to the used-pins list & persist the update
            _usedNumbers.Add(pin);
            lock (((ICollection)_usedNumbers).SyncRoot)
            {
                _usedNumberStore.SaveList(_usedNumbers);
            }
                
            return pin;
        }

        private bool IsPreviouslyUsed(string num)
        {
#if DEBUG
            if (_usedNumbers.Contains(num))
            {
                Console.Error.WriteLine($"Duplicate number {num} generated. Discarding...");
            }
#endif
            return _usedNumbers.Contains(num);
        }

        private bool IsValidNumber(string num)
        {
#if DEBUG
            if (_barredNumbers.Contains(num))
            {
                Console.Error.WriteLine($"Barred number {num} generated. Discarding...");
            }
#endif
            return _barredNumbers.Contains(num);

        }

        private void ClearUsedNumbers()
        {
            _usedNumberStore.DeleteStore();
            _usedNumbers.Clear();
        }
    }
}
