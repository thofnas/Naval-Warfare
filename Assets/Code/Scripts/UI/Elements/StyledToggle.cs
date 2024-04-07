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

        public StyledToggle(Theme theme, VisualElement parent, params string[] wrapperClasses)
        {
            parent.Add(this);
            this.AddClass(wrapperClasses);
            style.unityBackgroundImageTintColor = theme.MainColor;
            style.unityFontDefinition = new StyleFontDefinition(theme.BaseFont);
        }
    }
}