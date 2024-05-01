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
        public EventReference ShipMissedSound;

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, ThemeSettings, nameof(ThemeSettings));
            Validation.CheckIfNull(this, AITheme, nameof(AITheme));
            
            Validation.CheckIfNull(this, Music, nameof(Music));
            Validation.CheckIfNull(this, ShipHitSound, nameof(ShipHitSound));
            Validation.CheckIfNull(this, ShipMissedSound, nameof(ShipMissedSound));
        }
    }
}