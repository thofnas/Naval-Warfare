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

        public StyledButton(Theme theme, VisualElement parent, params string[] wrapperClasses) : this(
            theme, parent, null, wrapperClasses)
        {
        }

        public StyledButton(Theme theme, VisualElement parent, Action clickEvent,
            params string[] wrapperClasses) :
            base(clickEvent)
        {
            style.paddingBottom = 12;
            style.paddingTop = 12;
            style.paddingLeft = 16;
            style.paddingRight = 16;

            parent.Add(this);
            this.AddClass(wrapperClasses);
            style.unityBackgroundImageTintColor = theme.MainColor;
            style.unityFontDefinition = new StyleFontDefinition(theme.BaseFont);
        }
    }
}