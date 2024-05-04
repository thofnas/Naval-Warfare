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
            style.paddingBottom = 8;
            style.paddingTop = 8;
            style.paddingLeft = 14;
            style.paddingRight = 14;

            parent.Add(this);
            this.AddClass(wrapperClasses);
            style.unityBackgroundImageTintColor = theme.MainColor;
        }
    }
}