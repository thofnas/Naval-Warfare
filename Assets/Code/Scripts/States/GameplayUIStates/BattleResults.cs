using EventBus;
using Events;
using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.GameplayUIStates
{
    public class BattleResults : BaseState
    {
        private Label _resultsLabel;

        public BattleResults(GameplayUI gameplayUI, StyleSheet styleSheet) : base(gameplayUI)
        {
            var uiDocument = new GameObject(nameof(BattleResults)).AddComponent<UIDocument>();
            uiDocument.panelSettings = GameResources.Instance.UIDocumentPrefab.panelSettings;
            uiDocument.visualTreeAsset = GameResources.Instance.UIDocumentPrefab.visualTreeAsset;
            Root = uiDocument.rootVisualElement;

            Root.styleSheets.Add(styleSheet);

            GenerateView();

            var onAllCharactersShipsDestroyed =
                new EventBinding<OnAllCharactersShipsDestroyed>(OnAllCharactersShipsDestroyed);
            EventBus<OnAllCharactersShipsDestroyed>.Register(onAllCharactersShipsDestroyed);
        }

        protected sealed override VisualElement Root { get; }

        public sealed override void GenerateView()
        {
            SetVisible(false);
            
            VisualElement container = Root.CreateChild("container");
            VisualElement containerResults = container.CreateChild("container-results");
            VisualElement containerButtons = container.CreateChild("container-buttons");
            _resultsLabel = new Label();
            containerResults.Add(_resultsLabel);

            StyledButton restartButton = new(GameplayUI.ThemeSettings, containerButtons,
                () => SceneManager.LoadScene("Gameplay"))
            {
                text = "Restart"
            };

            StyledButton goToMainMenuButton = new(GameplayUI.ThemeSettings, containerButtons,
                () => SceneManager.LoadScene("MainMenu"))
            {
                text = "Main menu"
            };
        }

        protected override void SetVisible(bool value) => Root.visible = value;

        private void OnAllCharactersShipsDestroyed(OnAllCharactersShipsDestroyed obj) =>
            _resultsLabel.text = obj.LostCharacterType == CharacterType.Enemy
                ? "You won"
                : "You lost";
    }
}