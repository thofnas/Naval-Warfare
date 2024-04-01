using Themes;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledToggle : Toggle
    {
        private StyledToggle()
        {
        }

        public StyledToggle(ThemeSettings themeSettings, VisualElement parent, params string[] wrapperClasses)
        {
            parent.Add(this);
            this.AddClass(wrapperClasses);
            style.unityBackgroundImageTintColor = themeSettings.MainColor;
            style.unityFontDefinition = new StyleFontDefinition(themeSettings.BaseFont);
        }
    }
}