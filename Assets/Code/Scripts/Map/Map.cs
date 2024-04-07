using System.Collections.Generic;
using Themes;
using UnityEngine;
using Utilities;

namespace Map
{
    [CreateAssetMenu(fileName = nameof(Map) + "_")]
    public class Map : ScriptableObject
    {
        public List<Theme> ThemeSettings;
        public Theme AITheme;
        public MapType MapType = MapType.Islands;

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, ThemeSettings, nameof(ThemeSettings));
            Validation.CheckForNull(this, AITheme, nameof(AITheme));
        }
    }
}