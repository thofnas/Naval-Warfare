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

            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = TextData.BackButton
            };
        }

        private void CreateAudioSetting(VisualElement settingsContainer)
        {
            StyledPanel audioContainer = new(SelectedTheme.PlayerTheme, "audio-container");
            settingsContainer.Add(audioContainer);
            
            Label label = new(TextData.Audio);
            audioContainer.Add(label);
            
            VisualElement switches = audioContainer.CreateChild("audio-switches");
            
            VisualElement musicSwitch = switches.CreateChild("audio-switch"); 
            musicSwitch.Add(new Label(TextData.Music));
            StyledToggle musicToggle =
                new(SelectedTheme.PlayerTheme, musicSwitch, true, "music-switch");
            musicToggle.value = GameSettings.IsMusicEnabled();
            musicToggle.RegisterValueChangedCallback(e => ReadyToggle_OnValueChanged(AudioType.BGM, e.newValue));
            
            VisualElement sfxSwitch = switches.CreateChild("audio-switch"); 
            sfxSwitch.Add(new Label(TextData.Sfx));
            StyledToggle sfxToggle =
                new(SelectedTheme.PlayerTheme, sfxSwitch, true, "sfx-switch");
            sfxToggle.value = GameSettings.IsSfxEnabled();
            sfxToggle.RegisterValueChangedCallback(e => ReadyToggle_OnValueChanged(AudioType.Sfx, e.newValue));
            
            audioContainer.AddClass("setting-container");
        }

        private void CreateLanguageSetting(VisualElement settingsContainer)
        {
            StyledPanel languageContainer = new(SelectedTheme.PlayerTheme, "language-container");
            settingsContainer.Add(languageContainer);

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
            
            languageContainer.AddClass("setting-container");
        }

        private void CreateFPSSetting(VisualElement settingsContainer)
        {
            StyledPanel fpsContainer = new(SelectedTheme.PlayerTheme, "fps-container");
            settingsContainer.Add(fpsContainer);

            Label label = new(TextData.FramesPerSecond);
            fpsContainer.Add(label);
            
            GroupBox fpsGroupBox = fpsContainer.CreateChild<GroupBox>("fps-groupbox");

            foreach (int frameRate in new [] { 30, 60, 120, -1 })
            {
                string text = frameRate == -1
                    ? "Unlimited"
                    : frameRate.ToString();
                
                StyledRadioButton fpsButton = new(_mainMenuUIManager.SelectedTheme.PlayerTheme, fpsGroupBox) { text = text };
                fpsButton.RegisterValueChangedCallback(_ => GameSettings.SetFrameRate(frameRate));
                fpsButton.value = frameRate == GameSettings.GetTargetFrameRate();
            }
            
            fpsContainer.AddClass("setting-container");
        }

        private static void ReadyToggle_OnValueChanged(AudioType audioType, bool value)
        {
            EventBus<OnAudioSwitchValueChanged>.Invoke(new OnAudioSwitchValueChanged(audioType, value));
            switch (audioType)
            {
                case AudioType.BGM:
                    GameSettings.SetMusic(value);
                    break;
                case AudioType.Sfx:
                    GameSettings.SetSfx(value);
                    break;
                case AudioType.UI:
                    throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
            }
        }
    }
}