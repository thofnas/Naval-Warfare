using System;
using UnityEditor;
using UnityEngine;

namespace Utilities.Attributes
{
    public class Label : PropertyAttribute
    {
        private readonly string _label;

        public Label(string label)
        {
            _label = label;
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(Label))]
        public class ThisPropertyDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                try
                {
                    if (attribute is Label propertyAttribute)
                        label.text = propertyAttribute._label;

                    EditorGUI.PropertyField(position, property, label);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
#endif
    }
}