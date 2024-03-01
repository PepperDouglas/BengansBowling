using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BengansBowling.Utilities
{
    public static class FileStorageHelper
    {
        public static T ReadFromJsonFile<T>(string filePath) {
            if (!File.Exists(filePath)) {
                return default(T);
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { Converters = new[] { new ScoringStrategyConverter() } });
        }

        public static void WriteToJsonFile<T>(string filePath, T objectToWrite) {
            var json = JsonConvert.SerializeObject(objectToWrite, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new ScoringStrategyConverter() } });
            File.WriteAllText(filePath, json);
        }

    }

}
