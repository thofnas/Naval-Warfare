using UnityEngine;
using Utilities;
using VInspector;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(ThemeLibrary))]
    public class ThemeLibrary : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<IslandsThemeType, Theme> _islandsThemes = new();
        [SerializeField] private SerializedDictionary<OceanThemeType, Theme> _oceanThemes = new();

        public Theme GetTheme(IslandsThemeType islandsThemeType) => _islandsThemes[islandsThemeType];

        public Theme GetTheme(OceanThemeType oceanThemeType) => _oceanThemes[oceanThemeType];

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, _islandsThemes, nameof(_islandsThemes));
            Validation.CheckIfEmpty(this, _oceanThemes, nameof(_oceanThemes));
        }
    }
}