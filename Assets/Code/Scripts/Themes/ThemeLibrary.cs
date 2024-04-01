using UnityEngine;
using Utilities;
using VInspector;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(ThemeLibrary))]
    public class ThemeLibrary : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<IslandsTheme, ThemeSettings> _islandsThemes = new();

        public ThemeSettings GetTheme(IslandsTheme islandsTheme) => _islandsThemes[islandsTheme];

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, _islandsThemes, nameof(_islandsThemes));
        }
    }
}