using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Data;
using Misc;
using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class Settings : BaseState
    {
        private readonly MainMenuUIManager _mainMenuUIManager;
        private readonly GameSettings _gameSettings;

        public Settings(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet, GameSettings gameSettings) : base(mainMenuUIManager, stateMachine)
        {
            Root = VisualElementHelper.CreateDocument(nameof(Settings), styleSheet);

            _mainMenuUIManager = mainMenuUIManager;
            _gameSettings = gameSettings;
            
            GenerateUI();
        }

        protected override VisualElement Root { get; }

        protected sealed override void GenerateUI() 
        {
            VisualElement container = Root.CreateChild("container");

            StyledPanel settingsContainer = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, "settings-container");
            container.Add(settingsContainer);
            GenerateFPSSetting(settingsContainer);
            
            VisualElement languageContainer = settingsContainer.CreateChild("language-container");

            Label label = new(TextData.Language);
            languageContainer.Add(label);
            
            GroupBox languageGroupBox = languageContainer.CreateChild<GroupBox>("language-group-box");
            
            foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("lang"))
            {
                CultureInfo cultureInfo = new(textAsset.name);
                if (cultureInfo == null)
                    continue;

                StyledRadioButton radioButton = new(SelectedTheme.PlayerTheme, languageGroupBox)
                    { text = cultureInfo.NativeName };

                radioButton.RegisterValueChangedCallback(_ => GameSettings.SetLanguage(cultureInfo));
            }

            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = TextData.BackButton
            };
        }

        private void GenerateFPSSetting(VisualElement parent)
        {
            VisualElement fpsContainer = parent.CreateChild("fps-container");

            Label label = new(TextData.FramesPerSecond);
            fpsContainer.Add(label);
            
            GroupBox fpsGroupBox = fpsContainer.CreateChild<GroupBox>("fps-groupbox");
            
            StyledRadioButton fps30RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "30" };
            fps30RadioButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(30));
            
            StyledRadioButton fps60RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "60" };
            fps60RadioButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(60));
            
            StyledRadioButton fps120RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "120" };
            fps120RadioButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(120));

            StyledRadioButton fpsUnlimitedButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "Unlimited" };
            fpsUnlimitedButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(-1));
        }
    }
}