using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public class LanguageProvider
    {
        public const string FileExtension = ".json";
        public const string SearchPattern = "*" + FileExtension;
        public const string FolderName = "lang";
        public static readonly string LanguagePath;

        static LanguageProvider()
        {
            LanguagePath = Path.Combine(BetterStreamingAssets.Root, FolderName);
        }

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