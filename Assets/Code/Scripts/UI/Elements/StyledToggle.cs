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
            parent.Add(this);
            
            
            if (isSwitch)
                this.AddClass("switch");
                
            
            this.AddClass(wrapperClasses);
            style.unityBackgroundImageTintColor = theme.MainColor;
        }
    }
}