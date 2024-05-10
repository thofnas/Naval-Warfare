using EventBus;
using Events;
using Themes;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledRadioButton : RadioButton
    {
        private StyledRadioButton()
        {
        }
        
        public StyledRadioButton(Theme theme, VisualElement parent, bool initialValue,
            params string[] wrapperClasses) :
            base(null)
        {
            value = initialValue;
            
            parent.Add(this);
            
            foreach (VisualElement visualElement in Children()) 
                visualElement.style.unityBackgroundImageTintColor = theme.MainColor;
            
            this.AddClass(wrapperClasses);
            
            this.RegisterValueChangedCallback(e =>
            {
                if (e.previousValue == false)
                    return;
                
                EventBus<OnButtonClick>.Invoke(new OnButtonClick());
            });
        }
    }
}