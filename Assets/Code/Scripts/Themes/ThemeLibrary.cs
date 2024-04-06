using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Themes
{
    [Serializable]
    public class ThemeLibrary
    {
        [SerializeField] private List<IslandsThemeSettings> _islandsThemes = new();
        [SerializeField] private List<OceanThemeSetting> _oceanThemes = new();

        public IReadOnlyList<IslandsThemeSettings> IslandsThemes => _islandsThemes;
        public IReadOnlyList<OceanThemeSetting> OceanThemes => _oceanThemes;

        private void OnValidate()
        {
            Validation.CheckIfEmpty(null, _islandsThemes, nameof(_islandsThemes));
            Validation.CheckIfEmpty(null, _oceanThemes, nameof(_oceanThemes));
        }
    }
}