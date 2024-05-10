using EventBus;
using Events;
using Themes;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledToggle : Toggle
    {
        private StyledToggle()
        {
        }

        public StyledToggle(Theme theme, VisualElement parent, params string[] wrapperClasses) : this(theme, parent, false, false, wrapperClasses)
        {

        }

        public StyledToggle(Theme theme, VisualElement parent, bool initialValue, bool isSwitch, params string[] wrapperClasses)
        {
            value = initialValue;
            
            this.AddClass("styled-toggle");

            if (isSwitch)
                this.AddClass("switch");

            if (wrapperClasses is not null)
                this.AddClass(wrapperClasses);

            parent.Add(this);
                
            style.unityBackgroundImageTintColor = theme.MainColor;
            
            this.RegisterValueChangedCallback(_ => EventBus<OnButtonClick>.Invoke(new OnButtonClick(isSwitch: true)));
        }
    }
}