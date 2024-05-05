using Themes;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledRadioButton : RadioButton
    {
        private StyledRadioButton()
        {
        }
        
        public StyledRadioButton(Theme theme, VisualElement parent,
            params string[] wrapperClasses) :
            base(null)
        {
            parent.Add(this);
            
            foreach (VisualElement visualElement in Children()) 
                visualElement.style.unityBackgroundImageTintColor = theme.MainColor;
            
            this.AddClass(wrapperClasses);
        }
    }
}