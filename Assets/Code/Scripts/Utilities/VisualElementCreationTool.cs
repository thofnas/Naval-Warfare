using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Utilities
{
    public static class VisualElementCreationTool
    {
        public static VisualElement Create(params string[] classNames) => Create<VisualElement>(classNames);

        public static T Create<T>(params string[] classNames) where T : VisualElement, new()
        {
            var element = new T();

            foreach (string className in classNames) element.AddToClassList(className);

            return element;
        }

        public static void OnKeyDown(KeyDownEvent evt, Dictionary<Button, Action> buttonActions)
        {
            if (evt.keyCode != KeyCode.E) return;

            foreach (KeyValuePair<Button, Action> pair in buttonActions.Where(pair => pair.Key.IsFocused()))
            {
                pair.Value.Invoke();
                break;
            }
        }
    }
}