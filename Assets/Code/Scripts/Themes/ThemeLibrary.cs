using UnityEngine;
using Utilities;
using VInspector;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(ThemeLibrary))]
    public class ThemeLibrary : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<IslandsTheme, Theme> _islandsThemes = new();
        [SerializeField] private SerializedDictionary<OceanTheme, Theme> _oceanThemes = new();

        public Theme GetTheme(IslandsTheme islandsTheme) => _islandsThemes[islandsTheme];
        public Theme GetTheme(OceanTheme oceanTheme) => _oceanThemes[oceanTheme];

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, _islandsThemes, nameof(_islandsThemes));
            Validation.CheckIfEmpty(this, _oceanThemes, nameof(_oceanThemes));
        }
    }
}