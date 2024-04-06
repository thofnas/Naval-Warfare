using UnityEngine;
using Utilities;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(OceanThemeSetting))]
    public class OceanThemeSetting : ThemeSettings
    {
        public OceanTheme OceanTheme;

        protected override void OnValidate()
        {
            base.OnValidate();

            Validation.CheckForNull(this, OceanTheme, nameof(OceanTheme));
        }
    }
}