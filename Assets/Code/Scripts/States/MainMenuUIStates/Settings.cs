using System;
using System.Collections.Generic;
using UI;
using UI.Elements;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class Settings : BaseState
    {
        private readonly MainMenuUIManager _mainMenuUIManager;
        private readonly GameSettings _gameSettings;
        

        public Settings(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet, GameSettings gameSettings) : base(mainMenuUIManager, stateMachine)
        {
            Root = CreateDocument(nameof(Settings), styleSheet);

            _mainMenuUIManager = mainMenuUIManager;
            _gameSettings = gameSettings;
            
            GenerateView();
        }

        protected override VisualElement Root { get; }

        protected sealed override void GenerateView() 
        {
            base.GenerateView();
            
            VisualElement container = Root.CreateChild("container");

            StyledPanel settingsContainer = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, "settings-container");
            container.Add(settingsContainer);
            VisualElement fpsContainer = settingsContainer.CreateChild("fps-container");


            Label label = new("Frames Per Second");
            fpsContainer.Add(label);
            
            GroupBox fpsGroupBox = fpsContainer.CreateChild<GroupBox>("fps-groupbox");
            
            StyledRadioButton fps30RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "30" };
            fps30RadioButton.RegisterValueChangedCallback(_ => _gameSettings.SetFrameRate(30));
            fpsGroupBox.Add(fps30RadioButton);
            
            StyledRadioButton fps60RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "60" };
            fps60RadioButton.RegisterValueChangedCallback(_ => _gameSettings.SetFrameRate(60));
            fpsGroupBox.Add(fps60RadioButton);
            
            StyledRadioButton fps120RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "120" };
            fps120RadioButton.RegisterValueChangedCallback(_ => _gameSettings.SetFrameRate(120));
            fpsGroupBox.Add(fps120RadioButton);

            StyledRadioButton fpsUnlimitedButton =
                new StyledRadioButton(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "Unlimited" };
            fpsUnlimitedButton.RegisterValueChangedCallback(_ => _gameSettings.SetFrameRate(-1));
            fpsGroupBox.Add(fpsUnlimitedButton);

            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = "Back"
            };
        }
    }
}