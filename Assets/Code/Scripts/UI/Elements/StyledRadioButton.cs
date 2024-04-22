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
        
        public StyledRadioButton(Theme theme, VisualElement parent, params string[] wrapperClasses) : this(
            theme, parent, null, wrapperClasses)
        {
        }
        
        public StyledRadioButton(Theme theme, VisualElement parent, string label,
            params string[] wrapperClasses) :
            base(label)
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