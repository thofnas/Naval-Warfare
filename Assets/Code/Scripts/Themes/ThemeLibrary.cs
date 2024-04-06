using UnityEngine;
using Utilities;
using VInspector;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(ThemeLibrary))]
    public class ThemeLibrary : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<IslandsTheme, ThemeSettings> _islandsThemes = new();
        [SerializeField] private SerializedDictionary<OceanTheme, ThemeSettings> _oceanThemes = new();

        public ThemeSettings GetTheme(IslandsTheme islandsTheme) => _islandsThemes[islandsTheme];
        public ThemeSettings GetTheme(OceanTheme oceanTheme) => _oceanThemes[oceanTheme];

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, _islandsThemes, nameof(_islandsThemes));
            Validation.CheckIfEmpty(this, _oceanThemes, nameof(_oceanThemes));
        }
    }
}