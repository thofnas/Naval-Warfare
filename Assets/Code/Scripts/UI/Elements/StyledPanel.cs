using Themes;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StyledPanel : VisualElement
    {
        private StyledPanel() { }
        
        public StyledPanel(Theme theme) : this(theme, true, null) {}
        
        public StyledPanel(Theme theme, params string[] wrapperClasses) : this(theme, true, wrapperClasses) {}

        public StyledPanel(Theme theme, bool isSolid, params string[] wrapperClasses)
        {
            this.AddClass("styled-panel");
            
            if (!isSolid)
                this.AddClass("transparent");
            
            if (wrapperClasses is not null)
                this.AddClass(wrapperClasses);

            style.unityBackgroundImageTintColor = theme.MainColor;
        }
    }
}