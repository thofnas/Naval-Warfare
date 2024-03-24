using UnityEngine;
using Utilities;
using VInspector;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(ThemeLibrary))]
    public class ThemeLibrary : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<IslandsTheme, Theme> _islandsThemes = new();

        public Theme GetTheme(IslandsTheme islandsTheme) => _islandsThemes[islandsTheme];

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, _islandsThemes, nameof(_islandsThemes));
        }
    }
}