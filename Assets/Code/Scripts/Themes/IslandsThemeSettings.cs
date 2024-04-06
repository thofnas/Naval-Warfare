using UnityEngine;
using Utilities;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(IslandsThemeSettings))]
    public class IslandsThemeSettings : ThemeSettings
    {
        public IslandsTheme IslandsTheme;

        protected override void OnValidate()
        {
            base.OnValidate();

            Validation.CheckForNull(this, IslandsTheme, nameof(IslandsTheme));
        }
    }
}