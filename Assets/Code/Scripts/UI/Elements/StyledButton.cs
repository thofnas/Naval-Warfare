using System;
using Themes;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledButton : Button
    {
        public readonly VisualElement Wrapper;

        private StyledButton()
        {
        }

        public StyledButton(ThemeSettings themeSettings, VisualElement parent, params string[] wrapperClasses) : this(
            themeSettings, parent, null, wrapperClasses)
        {
        }

        public StyledButton(ThemeSettings themeSettings, VisualElement parent, Action clickEvent,
            params string[] wrapperClasses) :
            base(clickEvent)
        {
            style.paddingBottom = 12;
            style.paddingTop = 12;

            Wrapper = parent.CreateChild(wrapperClasses);
            Wrapper.AddClass(wrapperClasses);
            Wrapper.Add(this);
            Wrapper.style.unityBackgroundImageTintColor = themeSettings.MainColor;
            Wrapper.style.unityFontDefinition = new StyleFontDefinition(themeSettings.BaseFont);
        }
    }
}