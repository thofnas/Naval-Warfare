using UI;
using UI.Elements;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class Settings : BaseState
    {
        private readonly GameSettings _gameSettings;

        public Settings(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet, GameSettings gameSettings) : base(mainMenuUIManager, stateMachine)
        {
            Root = CreateDocument(nameof(Settings), styleSheet);

            _gameSettings = gameSettings;
            
            GenerateView();
        }

        protected override VisualElement Root { get; }

        protected sealed override void GenerateView() 
        {
            base.GenerateView();
            
            VisualElement container = Root.CreateChild("container");
            VisualElement settingsContainer = container.CreateChild("settings-container");
            VisualElement fpsContainer = settingsContainer.CreateChild("fps-container");

            StyledButton fps30Button =
                new(SelectedTheme.PlayerTheme, fpsContainer, () => _gameSettings.SetFrameRate(30))
                {
                    text = "30 fps"
                };
            StyledButton fps60Button =
                new(SelectedTheme.PlayerTheme, fpsContainer, () => _gameSettings.SetFrameRate(60))
                {
                    text = "60 fps"
                };
            StyledButton fps120Button =
                new(SelectedTheme.PlayerTheme, fpsContainer, () => _gameSettings.SetFrameRate(120))
                {
                    text = "120 fps"
                };
            
            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = "Back"
            };
        }
    }
}