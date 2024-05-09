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

        public StyledToggle(Theme theme, VisualElement parent, params string[] wrapperClasses) : this(theme, parent, false, wrapperClasses)
        {

        }

        public StyledToggle(Theme theme, VisualElement parent, bool isSwitch, params string[] wrapperClasses)
        {
            this.AddClass("styled-toggle");

            if (isSwitch)
                this.AddClass("switch");

            if (wrapperClasses is not null)
                this.AddClass(wrapperClasses);

            parent.Add(this);
                
            style.unityBackgroundImageTintColor = theme.MainColor;
        }
    }
}