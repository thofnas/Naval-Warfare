using System.Collections.Generic;
using UnityEngine;
using Utilities;
using VInspector;

namespace Map
{
    [CreateAssetMenu(fileName = nameof(MapLibrary))]
    public class MapLibrary : ScriptableObject
    {
        public IReadOnlyDictionary<MapType, Map> Maps => _maps;
        
        [SerializeField] private SerializedDictionary<MapType, Map> _maps;
        
        private void OnValidate()
        {
            Validation.CheckDictionaryByProperty(_maps, pair => pair.Key == pair.Value.MapType, nameof(_maps));
        }  
    }
    
}