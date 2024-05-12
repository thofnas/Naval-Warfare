using UnityEngine.UIElements;

namespace Utilities.Extensions
{
    public static class VisualElementExtensions
    {
        public static VisualElement CreateChild(this VisualElement parent, params string[] classes)
        {
            var child = new VisualElement();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        public static T CreateChild<T>(this VisualElement parent, params string[] classes)
            where T : VisualElement, new()
        {
            var child = new T();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement
        {
            parent.Add(child);
            return child;
        }

        public static T AddClass<T>(this T visualElement, params string[] classes) where T : VisualElement
        {
            foreach (string cls in classes)
                if (!string.IsNullOrEmpty(cls))
                    visualElement.AddToClassList(cls);
            return visualElement;
        }

        public static T WithManipulator<T>(this T visualElement, IManipulator manipulator) where T : VisualElement
        {
            visualElement.AddManipulator(manipulator);
            return visualElement;
        }
        
        public static void AnimateOpacity<T>(this T visualElement, float from, float to, int durationMs) where T : VisualElement =>
            visualElement.experimental.animation.Start(from, to, durationMs, (element, value) =>
                element.style.opacity = new StyleFloat(value));
    }
}