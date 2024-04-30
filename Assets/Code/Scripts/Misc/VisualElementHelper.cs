using UnityEngine;
using UnityEngine.UIElements;

namespace Misc
{
    public static class VisualElementHelper
    {
        public static VisualElement CreateDocument(string name, StyleSheet styleSheet)
        {
            var uiDocument = new GameObject(name).AddComponent<UIDocument>();
            uiDocument.panelSettings = GameResources.Instance.UIDocumentPrefab.panelSettings;
            uiDocument.visualTreeAsset = GameResources.Instance.UIDocumentPrefab.visualTreeAsset;
            VisualElement root = uiDocument.rootVisualElement;

            root.styleSheets.Add(styleSheet);

            root.visible = false;
            
            return root;
        }
    }
}