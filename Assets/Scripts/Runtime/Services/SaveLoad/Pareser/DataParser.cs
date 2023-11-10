using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace SA.Runtime.Services.SaveLoad.Parser
{
    public static class DataParser
    {
        private static bool useEncryption = false;
        private static readonly string encryptionCodeWord = "crecpecfecs";

        public static async UniTask<Dictionary<string, object>> DeserializeAsync(string dataDirPath, string fileName)
        {
            string fullPath = Path.Combine(dataDirPath, fileName);

            Dictionary<string, object> loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    // load the serialized data from the file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = await reader.ReadToEndAsync();
                        }
                    }

                    // optionally decrypt the data
                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    // deserialize the data from Json back into the C# object
                    loadedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load file at path: "
                            + fullPath + " and backup did not work.\n" + e);
                }
            }

            return loadedData;
        }


        public static async UniTask SerializeAsync(string dataDirPath, string fileName, Dictionary<string, object> data)
        {
            // use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath,  fileName);

            try
            {
                // create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // serialize the C# game data object into Json
                string dataToStore = JsonConvert.SerializeObject(data);

                // optionally encrypt the data
                if (useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                // write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        await writer.WriteAsync(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }


        // the below is a simple implementation of XOR encryption
        private static string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }
            return modifiedData;
        }
    }
}