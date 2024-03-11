using UnityEngine.UIElements;

namespace Utilities
{
    public static class ButtonUIToolkitExtensions
    {
        public static bool IsFocused(this Button button) => button.focusController.focusedElement == button;
    }
}