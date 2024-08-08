using System.IO;
using Newtonsoft.Json;

namespace Data
{
    public class LanguageProvider
    {
        public const string FileExtension = ".json";
        public const string SearchPattern = "*" + FileExtension;
        public const string FolderName = "lang";

        public bool TryLoad(string fileName, out TextData textData)
        {
            textData = null;

            string filePath = Path.Combine(FolderName, $"{fileName}{FileExtension}");

            if (!BetterStreamingAssets.FileExists(filePath))
                return false;

            textData = JsonConvert.DeserializeObject<TextData>(BetterStreamingAssets.ReadAllText(filePath));

            return true;
        }
    }
}