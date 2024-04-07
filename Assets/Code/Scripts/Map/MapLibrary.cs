using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utilities;
using VInspector;

namespace Map
{
    [CreateAssetMenu(fileName = nameof(MapLibrary))]
    public class MapLibrary : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<MapType, Map> _maps;

        private void OnValidate()
        {
            Validation.CheckDictionaryByProperty(_maps, pair => pair.Key == pair.Value.MapType, nameof(_maps));
        }  
    }
    
}