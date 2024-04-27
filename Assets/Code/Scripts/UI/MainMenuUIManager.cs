using AchievementSystem;
using Infrastructure;
using States.MainMenuUIStates;
using Themes;
using Themes.Store;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class MainMenuUIManager : MonoBehaviour
    {
        public SelectedTheme SelectedTheme { get; private set; }
        public StorePanel.Factory StorePanelFactory { get; private set; }

        public ThemeUnlocker ThemeUnlocker { get; set; }
        public ThemeSelector ThemeSelector { get; set; }
        public ThemeUnlocker Theme { get; set; }
        public OwnedThemesChecker OwnedThemesChecker { get; set; }
        public SelectedThemeChecker SelectedThemeChecker { get; set; }
        
        
        // states
        public MainMenu MainMenuState { get; private set; }
        public Store StoreState { get; private set; }
        public Settings SettingsState { get; private set; }

        [SerializeField] private StyleSheet _mainMenuStyleSheet;
        [SerializeField] private StyleSheet _storeStyleSheet;
        [SerializeField] private StyleSheet _optionsStyleSheet;
        [SerializeField] private StyleSheet _achievementsStyleSheet;
        
        [SerializeField] private StoreContent _storeContent;
        
        private Wallet _wallet;

        private StateMachine.StateMachine _stateMachine;
        private GameSettings _gameSettings;
        private AchievementStorage _achievementStorage;

        [Inject]
        private void Construct(StateMachine.StateMachine stateMachine, 
            SelectedTheme selectedTheme,
            StorePanel.Factory storePanelFactory, 
            Wallet wallet, 
            ThemeUnlocker themeUnlocker,
            ThemeSelector themeSelector, 
            ThemeUnlocker theme, 
            OwnedThemesChecker ownedThemesChecker, 
            SelectedThemeChecker selectedThemeChecker,
            GameSettings gameSettings,
            AchievementStorage achievementStorage)
        {
            _stateMachine = stateMachine;
            SelectedTheme = selectedTheme;
            StorePanelFactory = storePanelFactory;
            _wallet = wallet;
            ThemeUnlocker = themeUnlocker;
            ThemeSelector = themeSelector;
            Theme = theme;
            OwnedThemesChecker = ownedThemesChecker;
            SelectedThemeChecker = selectedThemeChecker;
            _gameSettings = gameSettings;
            _achievementStorage = achievementStorage;
        }

        private void Awake()
        {
            MainMenuState = new MainMenu(this, _stateMachine, _mainMenuStyleSheet);
            StoreState = new Store(this, _stateMachine, _storeStyleSheet, _storeContent, _wallet);
            SettingsState = new Settings(this, _stateMachine, _optionsStyleSheet, _gameSettings);
            AchievementsState =
                new States.MainMenuUIStates.Achievements(this, _stateMachine, _achievementsStyleSheet,
                    _achievementStorage);
                
            _stateMachine.SetState(MainMenuState);
        }

        public States.MainMenuUIStates.Achievements AchievementsState { get; set; }
    }
}