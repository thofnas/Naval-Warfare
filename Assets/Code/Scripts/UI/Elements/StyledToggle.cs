using Themes;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledToggle : Toggle
    {
        public readonly VisualElement Wrapper;

        private StyledToggle()
        {
        }

        public StyledToggle(ThemeSettings themeSettings, VisualElement parent, params string[] wrapperClasses)
        {
            Wrapper = parent.CreateChild(wrapperClasses);
            Wrapper.AddClass(wrapperClasses);
            Wrapper.Add(this);
            Wrapper.style.unityBackgroundImageTintColor = themeSettings.MainColor;
            Wrapper.style.unityFontDefinition = new StyleFontDefinition(themeSettings.BaseFont);
        }
    }
}