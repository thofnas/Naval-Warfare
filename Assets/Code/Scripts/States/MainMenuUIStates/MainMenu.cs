using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class MainMenu : BaseState
    {
        public MainMenu(MainMenuUI mainMenuUI, StyleSheet styleSheet, StateMachine.StateMachine stateMachine) : base(mainMenuUI, stateMachine)
        {
            var uiDocument = new GameObject(nameof(MainMenuUI)).AddComponent<UIDocument>();
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

            VisualElement titleContainer = container.CreateChild("title-container");
            titleContainer.Add(new Label("Battleship"));

            VisualElement buttonsContainer = container.CreateChild("buttons-container");

            StyledButton startGameButton = new(MainMenuUIInstance.ThemeSettings, buttonsContainer,
                () => SceneManager.LoadScene("Gameplay"), "start-button")
            {
                text = "Start Game"
            };

            StyledButton optionsButton = new(MainMenuUIInstance.ThemeSettings, buttonsContainer, "options-button")
            {
                text = "Options"
            };
        }

        protected override void SetVisible(bool value) => Root.visible = value;
    }
}