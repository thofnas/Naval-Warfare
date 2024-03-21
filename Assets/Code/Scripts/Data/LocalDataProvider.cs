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

        private PlayerData _playerData;

        [Inject]
        public LocalDataProvider(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private string SavePath => Application.persistentDataPath;
        
        private string FullPath => Path.Combine(SavePath, $"{FileName}{FileExtension}");

        public bool TryLoad()
        {
            if (IsDataAlreadyExists() == false)
                return false;

            _playerData = JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(FullPath));
            return true;
        }

        public void Save()
        {
            File.WriteAllText(FullPath, JsonConvert.SerializeObject(_playerData, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        private bool IsDataAlreadyExists() => File.Exists(FullPath);
    }
}