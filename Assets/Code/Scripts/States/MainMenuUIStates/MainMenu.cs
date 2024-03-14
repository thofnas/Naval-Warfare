using UI;
using UI.Elements;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class MainMenu : BaseState
    {
        public MainMenu(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet) : base(mainMenuUIManager, stateMachine)
        {
            Root = CreateDocument(nameof(MainMenu), styleSheet);

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

            StyledButton startGameButton = new(SelectedThemeSettings.PlayerThemeSettings,
                buttonsContainer,
                () => SceneManager.LoadScene("Gameplay"),
                "start-button")
            {
                text = "Start Game"
            };
            
            StyledButton storeButton = new(SelectedThemeSettings.PlayerThemeSettings, 
                buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.StoreState), 
                "start-button")
            {
                text = "Store"
            };

            StyledButton optionsButton = new(SelectedThemeSettings.PlayerThemeSettings, 
                buttonsContainer, 
                () => StateMachine.SwitchState(MainMenuUIManager.OptionsState), 
                "options-button")
            {
                text = "Options"
            };
        }
    }
}