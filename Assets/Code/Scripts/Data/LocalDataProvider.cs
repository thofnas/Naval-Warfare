using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Data
{
    public class LocalDataProvider
    {
        private const string FileName = "PlayerSave";
        private const string FileExtension = ".json";

        private PersistentData _persistentData;

        [Inject]
        public LocalDataProvider(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        private string SavePath => Application.persistentDataPath;
        
        private string FullPath => Path.Combine(SavePath, $"{FileName}{FileExtension}");

        public bool TryLoad()
        {
            if (!IsDataAlreadyExists())
                return false;

            _persistentData = JsonConvert.DeserializeObject<PersistentData>(File.ReadAllText(FullPath));
            return true;
        }

        public void Save()
        {
            File.WriteAllText(FullPath, JsonConvert.SerializeObject(_persistentData, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        private bool IsDataAlreadyExists() => File.Exists(FullPath);
    }
}