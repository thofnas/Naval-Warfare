﻿using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public class LocalDataProvider : ILocalDataLoader
    {
        private const string FileName = "PlayerSave";
        private const string FileExtension = ".json";

        private PersistentData _persistentData;

        public LocalDataProvider(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        private static string SavePath => Application.persistentDataPath;
        
        private static string FullPath => Path.Combine(SavePath, $"{FileName}{FileExtension}");

        public bool TryLoad(out PersistentData loadedData)
        {
            loadedData = null;
            
            if (!IsDataAlreadyExists())
                return false;

            _persistentData = JsonConvert.DeserializeObject<PersistentData>(File.ReadAllText(FullPath));

            loadedData = _persistentData;
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