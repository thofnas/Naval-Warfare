using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public class LanguageProvider
    {
        private const string FileExtension = ".json";
        public static string LanguagePath => Path.Combine(Application.dataPath, "Resources/lang/");

        public bool TryLoad(string fileName, out TextData textData)
        {
            textData = null;
            
            string filePath = Path.Combine(LanguagePath, $"{fileName}{FileExtension}");

            if (!File.Exists(filePath))
                return false;

            textData = JsonConvert.DeserializeObject<TextData>(File.ReadAllText(filePath));
            return true;
        }
    }
}