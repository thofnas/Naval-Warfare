using System;
using System.Collections.Generic;
using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class Settings : BaseState
    {
        private static readonly List<string> s_radioButtonChoices = new() { "30", "60", "120" };
        
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

            RadioButtonGroup radioButtonGroup = new("Frames Per Second", s_radioButtonChoices);
            radioButtonGroup.RegisterValueChangedCallback(e => _gameSettings.SetFrameRate(int.Parse(s_radioButtonChoices[e.newValue])));
            radioButtonGroup.Focus();
            settingsContainer.Add(radioButtonGroup);
            
            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = "Back"
            };
        }
    }
}