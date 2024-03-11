using UI;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.GameplayUIStates
{
    public class Battle : BaseState
    {
        public Battle(GameplayUIManager gameplayUIManager, StyleSheet styleSheet) : base(gameplayUIManager)
        {
            var uiDocument = new GameObject(nameof(Battle)).AddComponent<UIDocument>();
            uiDocument.panelSettings = GameResources.Instance.UIDocumentPrefab.panelSettings;
            uiDocument.visualTreeAsset = GameResources.Instance.UIDocumentPrefab.visualTreeAsset;
            Root = uiDocument.rootVisualElement;

            Root.styleSheets.Add(styleSheet);
        }

        protected sealed override VisualElement Root { get; }

        protected override void GenerateView()
        {
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