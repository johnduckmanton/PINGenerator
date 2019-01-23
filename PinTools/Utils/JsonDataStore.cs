using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PinTools.Utils
{
    /// <summary>
    /// Simple JSON format data store
    /// </summary>
    class JsonDataStore
    {
        // Default filename for the stored data file
        private readonly string _filePath = @".\data.json";
        
        public JsonDataStore()
        {

        }

        public JsonDataStore(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Save the list to a JSON format file
        /// </summary>
        /// <param name="list"></param>
        public void SaveList(ICollection list)
        {
            // serialize JSON to a string and then write string to a file
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(list));

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(_filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, list);
            }
        }

        /// <summary>
        /// Load a JSON format file into a list
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns>list of values</returns>
        public List<string> LoadList()
        {
            List<string> list = new List<string>();

            if (File.Exists(_filePath))
            {
                String JSONtxt = File.ReadAllText(_filePath);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(JSONtxt);
            }

            return list;
        }

        public void DeleteStore()
        {
            File.Delete(_filePath);
        }

        public bool StoreExists()
        {
            return File.Exists(_filePath);
        }
    }
}
