using UI;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.GameplayUIStates
{
    public class Battle : BaseState
    {
        public Battle(GameplayUI gameplayUI, StyleSheet styleSheet) : base(gameplayUI)
        {
            var uiDocument = new GameObject(nameof(Battle)).AddComponent<UIDocument>();
            uiDocument.panelSettings = GameResources.Instance.UIDocumentPrefab.panelSettings;
            uiDocument.visualTreeAsset = GameResources.Instance.UIDocumentPrefab.visualTreeAsset;
            Root = uiDocument.rootVisualElement;

            Root.styleSheets.Add(styleSheet);

            GenerateView();
        }

        protected sealed override VisualElement Root { get; }

        public sealed override void GenerateView()
        {
            SetVisible(false);
            
            VisualElement container = Root.CreateChild("container");
            VisualElement charactersDiv = container.CreateChild("characters-container");
            VisualElement characterPlayerDiv = charactersDiv.CreateChild("character-container");
            VisualElement characterEnemyDiv = charactersDiv.CreateChild("character-container");

            characterPlayerDiv.Add(new Label("YOU"));
            characterEnemyDiv.Add(new Label("ENEMY"));
        }

        protected override void SetVisible(bool value) => Root.visible = value;
    }
}