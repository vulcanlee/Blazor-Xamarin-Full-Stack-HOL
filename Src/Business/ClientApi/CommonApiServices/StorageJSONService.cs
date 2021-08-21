using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi.CommonApiServices
{
    public class StorageJSONService<T>
    {

        /// <summary>
        /// Loads data from a file
        /// </summary>
        /// <param name="fileName">Name of the file to read.</param>
        /// <returns>Data object</returns>
        public static async Task<T> ReadFromFileAsync(string directoryName, string fileName)
        {
            //T loadedFile = default(T);
            T loadedFile = (T)Activator.CreateInstance(typeof(T));
            string tempStr = "";
            try
            {
                tempStr = await StorageUtility.ReadFromDataFileAsync(directoryName, fileName);
                loadedFile = JsonConvert.DeserializeObject<T>(tempStr);
            }
            catch
            {
                //ApplicationState.ErrorLog.Add(new ErrorLog("LoadFromFile", e.Message));
            }

            return loadedFile;
        }

        public static T LoadFromString(string SourceString)
        {
            T loadedFile = (T)Activator.CreateInstance(typeof(T));
            try
            {
                loadedFile = JsonConvert.DeserializeObject<T>(SourceString);
            }
            catch
            {
                //ApplicationState.ErrorLog.Add(new ErrorLog("LoadFromFile", e.Message));
            }

            return loadedFile;
        }

        /// <summary>
        /// Saves data to a file.
        /// </summary>
        /// <param name="fileName">Name of the file to write to</param>
        /// <param name="data">The data to save</param>
        public static async Task WriteToDataFileAsync(string directoryName, string fileName, T data)
        {
            try
            {
                string output = JsonConvert.SerializeObject(data);
                await StorageUtility.WriteToDataFileAsync(directoryName, fileName, output);
            }
            catch
            {
                // Add desired error handling for your application
                // ApplicationState.ErrorLog.Add(new ErrorLog("SaveToFile", e.Message));
            }
        }

    }
}
