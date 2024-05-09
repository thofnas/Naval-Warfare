using System;
using System.Globalization;
using EventBus;
using Events;
using Misc;
using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;
using AudioType = Audio.AudioType;

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

            StyledPanel settingsPanel = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, "settings-panel");
            container.Add(settingsPanel);
            ScrollView settingsScrollView = new(ScrollViewMode.Vertical)
            {
                touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic,
                verticalScrollerVisibility = ScrollerVisibility.Hidden
            };
            settingsPanel.Add(settingsScrollView);
            
            CreateAudioSetting(settingsScrollView);
            CreateFPSSetting(settingsScrollView);
            CreateLanguageSetting(settingsScrollView);
            
            foreach (VisualElement visualElement in settingsPanel.Children())
            {
                visualElement.AddClass("setting-container");
            }

            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = TextData.BackButton
            };
        }

        private void CreateAudioSetting(VisualElement settingsContainer)
        {
            VisualElement audioContainer = settingsContainer.CreateChild("audio-container");
            
            Label label = new(TextData.Audio);
            audioContainer.Add(label);
            
            VisualElement switches = audioContainer.CreateChild("audio-switches");
            VisualElement musicSwitch = switches.CreateChild("audio-switch"); 
            musicSwitch.Add(new Label(TextData.Music));
            StyledToggle musicToggle =
                new(SelectedTheme.PlayerTheme, musicSwitch, true, "music-switch");
            musicToggle.RegisterValueChangedCallback(e => ReadyToggle_OnValueChanged(AudioType.BGM, e.newValue));
        }

        private void CreateLanguageSetting(VisualElement settingsContainer)
        {
            VisualElement languageContainer = settingsContainer.CreateChild("language-container");

            Label label = new(TextData.Language);
            languageContainer.Add(label);
            
            GroupBox languageGroupBox = languageContainer.CreateChild<GroupBox>("language-group-box");
            
            foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("lang"))
            {
                CultureInfo cultureInfo = new(textAsset.name);
                if (cultureInfo == null)
                    continue;

                StyledRadioButton languageRadioButton = new(SelectedTheme.PlayerTheme, languageGroupBox, "language-radio-button")
                    { 
                        text = cultureInfo.NativeName,
                        value = textAsset.name == GameSettings.GetLanguage()
                    };

                languageRadioButton.RegisterValueChangedCallback(_ => GameSettings.SetLanguage(cultureInfo));
            }
        }

        private void CreateFPSSetting(VisualElement parent)
        {
            VisualElement fpsContainer = parent.CreateChild("fps-container");

            Label label = new(TextData.FramesPerSecond);
            fpsContainer.Add(label);
            
            GroupBox fpsGroupBox = fpsContainer.CreateChild<GroupBox>("fps-groupbox");
            
            StyledRadioButton fps30RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "30" };
            fps30RadioButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(30));
            fps30RadioButton.value = 30 == GameSettings.GetTargetFrameRate();
            
            StyledRadioButton fps60RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "60" };
            fps60RadioButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(60));
            fps60RadioButton.value = 60 == GameSettings.GetTargetFrameRate();
            
            StyledRadioButton fps120RadioButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "120" };
            fps120RadioButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(120));
            fps120RadioButton.value = 120 == GameSettings.GetTargetFrameRate();

            StyledRadioButton fpsUnlimitedButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = "Unlimited" };
            fpsUnlimitedButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(-1));
            fpsUnlimitedButton.value = -1 == GameSettings.GetTargetFrameRate();
        }

        private static void ReadyToggle_OnValueChanged(AudioType audioType, bool value)
        {
            EventBus<OnAudioSwitchValueChanged>.Invoke(new OnAudioSwitchValueChanged(audioType, value));
        }
    }
}