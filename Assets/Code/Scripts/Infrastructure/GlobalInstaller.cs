using System;
using AchievementSystem;
using AchievementSystem.Achievements;
using Audio;
using Data;
using FMODUnity;
using Map;
using Misc;
using Themes;
using Themes.Store;
using UnityEngine;
using Zenject;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Infrastructure
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private ThemeLibrary _themeLibrary;
        [SerializeField] private MapLibrary _mapLibrary;
        [SerializeField] private EventReference _mainMenuMusic;
        private PersistentData _persistentData;
        private LocalDataProvider _localDataProvider;
        private Wallet _wallet;
        private LanguageProvider _languageProvider;
        private LanguageData _languageData;
        private readonly StoreContent _storeContent;
        private ThemeUnlocker _themeUnlocker;

        [Inject]
        private GlobalInstaller(StoreContent storeContent)
        {
            _storeContent = storeContent;
        }

        public override void InstallBindings()
        {
            _persistentData = new PersistentData();
            _languageProvider = new LanguageProvider();
            _languageData = new LanguageData(_languageProvider);
            _localDataProvider = new LocalDataProvider(_persistentData);
            new DataSaver(_localDataProvider);
            
            LoadDataOrInit();
            Bind();
        }

        private void Bind()
        {
            DataLoader();

            StateMachine();
            AsyncProcessor();
            UnityMainThread();

            ThemeVisitors();
            Wallet();
            PersistentData();
            LanguageData();
            SelectedTheme();
            MapLibrary();
            MainMenuMusicManager();
            GameSettings();
            Achievements();
        }

        private void LanguageData() => Container.BindInstance(_languageData);

        private void GameSettings() => Container.BindInterfacesAndSelfTo<GameSettings>().AsSingle().NonLazy();

        private void MainMenuMusicManager() => Container.BindInterfacesTo<MainMenuMusicManager>().AsSingle().WithArguments(_mainMenuMusic);

        private void MapLibrary() => Container.BindInstance(_mapLibrary);

        private void Achievements()
        {
            FirstMapBought firstMapBought = new(_persistentData, "91dfcd2c-715e-41dc-9808-c106e42f6127", _wallet, _languageData);
            WinTenTimes winTenTimes = new(_persistentData, "c74397d2-9fa4-452f-aadc-13ad8f47576b", _languageData, _storeContent, _themeUnlocker);
            
            AchievementStorage achievementStorage = new(firstMapBought, winTenTimes);

            Container.Bind<FirstMapBought>().FromInstance(firstMapBought).AsSingle();
            Container.Bind<WinTenTimes>().FromInstance(winTenTimes).AsSingle();
            Container.Bind<AchievementStorage>().FromInstance(achievementStorage).AsSingle().NonLazy();
        }

        private void PersistentData() => Container.BindInstance(_persistentData);

        private void DataLoader() => Container.Bind<ILocalDataLoader>().FromInstance(_localDataProvider).AsSingle().NonLazy();

        private void LoadDataOrInit()
        {
            if (!_localDataProvider.TryLoad(out PersistentData loadedData))
            {
                _persistentData.PlayerData = new PlayerData();
                _persistentData.PlayerStatistics = new PlayerStatistics();
                return;
            }

            _persistentData = loadedData;
        }

        private void Wallet()
        {
            _wallet = new Wallet(_persistentData);
            Container.Bind<Wallet>().FromInstance(_wallet).AsSingle();
        }

        private void ThemeVisitors()
        {
            _themeUnlocker = new ThemeUnlocker(_persistentData);
            
            Container.Bind<ThemeSelector>().AsSingle().WithArguments(_persistentData);
            Container.Bind<ThemeUnlocker>().FromInstance(_themeUnlocker);
            Container.Bind<SelectedThemeChecker>().AsTransient().WithArguments(_persistentData);
            Container.Bind<OwnedThemesChecker>().AsTransient().WithArguments(_persistentData);
        }

        private void UnityMainThread() => Container.Bind<UnityMainThread>().FromNewComponentOnNewGameObject().AsSingle();

        private void StateMachine() => Container.BindInterfacesAndSelfTo<StateMachine.StateMachine>().AsTransient();
        
        private void AsyncProcessor() => Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();

        private void SelectedTheme()
        {
            Theme theme = _persistentData.PlayerData.SelectedMapType switch
            {
                MapType.Islands => _themeLibrary.GetTheme(_persistentData.PlayerData.SelectedIslandsThemeType),
                MapType.Ocean => _themeLibrary.GetTheme(_persistentData.PlayerData.SelectedOceanThemeType),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var selectedTheme = new SelectedTheme(theme);
            Container.BindInstance(selectedTheme);
            Container.BindInstance(selectedTheme.PlayerTheme);
        }
    }
}