using CodeShared.methods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeShared
{
    public class SettingManager
    {
        public static string FileDir { get; set; }

        public static bool FileExist()
        {
            string file = Path.Combine(FileDir, "values.json");
            return File.Exists(file);
        }
        public static void SaveData()
        {
            try
            {
                Directory.CreateDirectory(FileDir);

                string Out = JsonConvert.SerializeObject(Core.login);
                File.WriteAllText(Path.Combine(FileDir, "values.json"), Out);
            }
            catch (FileNotFoundException ex) { throw ex; }

            catch
            {
                throw new ErrorInGettingTheData("Error in saving the data");
            }
        }

        internal static Login TryToLoad()
        {
            try
            {
                return GetData();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error in getting the values : using null");
                return null;
            }
        }

        public class ErrorInGettingTheData : Exception
        {
            public ErrorInGettingTheData() : base() { }
            public ErrorInGettingTheData(string message) : base(message) { }
        }

        internal static void DeleteData()
        {
            File.Delete(Path.Combine(FileDir, "values.json"));
            
        }

        public static Login GetData()
        {
            try
            {
                string Json = File.ReadAllText(Path.Combine(FileDir, "values.json"));
                Login data = JsonConvert.DeserializeObject<Login>(Json);
                return data;
            }
            catch
            {
                throw new ErrorInGettingTheData();
            }
        }

    }
}

