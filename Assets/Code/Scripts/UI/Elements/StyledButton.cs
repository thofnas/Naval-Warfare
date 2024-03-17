using System;
using Themes;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledButton : Button
    {
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
            style.paddingLeft = 16;
            style.paddingRight = 16;

            parent.Add(this);
            this.AddClass(wrapperClasses);
            style.unityBackgroundImageTintColor = themeSettings.MainColor;
            style.unityFontDefinition = new StyleFontDefinition(themeSettings.BaseFont);
        }
    }
}