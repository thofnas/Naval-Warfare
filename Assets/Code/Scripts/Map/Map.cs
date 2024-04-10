using System.Collections.Generic;
using FMODUnity;
using Themes;
using UnityEngine;
using Utilities;

namespace Map
{
    [CreateAssetMenu(fileName = nameof(Map) + "_")]
    public class Map : ScriptableObject
    {
        public MapType MapType = MapType.Islands;
        
        [Header("Themes")]
        public List<Theme> ThemeSettings;
        public Theme AITheme;

        [Header("Audio")] 
        public EventReference Music;
        public EventReference ShipHitSound;

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, ThemeSettings, nameof(ThemeSettings));
            Validation.CheckForNull(this, AITheme, nameof(AITheme));
            
            Validation.CheckForNull(this, Music, nameof(Music));
            Validation.CheckForNull(this, ShipHitSound, nameof(ShipHitSound));
        }
    }
}