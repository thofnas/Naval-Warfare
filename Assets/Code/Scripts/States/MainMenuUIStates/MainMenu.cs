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

        protected sealed override void GenerateView() 
        {
            base.GenerateView();
            
            VisualElement container = Root.CreateChild("container");
            VisualElement contentContainer = container.CreateChild("content-container");

            VisualElement titleContainer = contentContainer.CreateChild("title-container", "flex-center");
            titleContainer.Add(new Label("Naval Warfare"));

            VisualElement buttonsContainer = contentContainer.CreateChild("buttons-container");

            StyledButton startGameButton = new(SelectedThemeSettings.PlayerTheme,
                buttonsContainer,
                () => SceneManager.LoadScene("Gameplay"),
                "start-button")
            {
                text = "Start Game"
            };
            
            StyledButton storeButton = new(SelectedThemeSettings.PlayerTheme, 
                buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.StoreState), 
                "store-button")
            {
                text = "Store"
            };

            StyledButton optionsButton = new(SelectedThemeSettings.PlayerTheme, 
                buttonsContainer, 
                () => StateMachine.SwitchState(MainMenuUIManager.OptionsState), 
                "options-button")
            {
                text = "Options"
            };
        }
    }
}