using CodeShared.methods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeShared
{
    public class settingManager
    {
        public static string fileDir { get; set; }

        public static bool FileExist()
        {
            string file = Path.Combine(fileDir, "values.json");
            return File.Exists(file);
        }
        public static void saveData()
        {
            try
            {
                Directory.CreateDirectory(fileDir);

                string Out = JsonConvert.SerializeObject(Core.login);
                File.WriteAllText(Path.Combine(fileDir, "values.json"), Out);
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
                return getData();
            }
            catch
            {
                return null;
            }
        }

        public class ErrorInGettingTheData : Exception
        {
            public ErrorInGettingTheData() : base() { }
            public ErrorInGettingTheData(string message) : base(message) { }
        }
        public static Login getData()
        {
            try
            {
                string Json = File.ReadAllText(Path.Combine(fileDir, "values.json"));
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

