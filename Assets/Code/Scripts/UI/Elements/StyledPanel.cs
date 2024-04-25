using Themes;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledPanel : VisualElement
    {
        private StyledPanel() { }
        
        public StyledPanel(Theme theme) : this(theme, null) {}
        
        public StyledPanel(Theme theme, params string[] wrapperClasses)
        {
            this.AddClass("styled-panel");
            
            if (wrapperClasses is not null)
                this.AddClass(wrapperClasses);

            style.unityBackgroundImageTintColor = theme.MainColor;
            style.unityFontDefinition = new StyleFontDefinition(theme.BaseFont);
        }
    }
}