using System;
using System.Collections.Generic;
using AchievementSystem;
using AchievementSystem.Achievements;
using Audio;
using Data;
using EventBus;
using FMODUnity;
using Map;
using Misc;
using Rewards;
using Themes;
using Themes.Store;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private ThemeLibrary _themeLibrary;
        [SerializeField] private MapLibrary _mapLibrary;
        [SerializeField] private EventReference _mainMenuMusic;
        private PersistentData _persistentData;
        private LocalDataProvider _dataProvider;
        private Wallet _wallet;

        public override void InstallBindings()
        {
            _persistentData = new PersistentData();
            _dataProvider = new LocalDataProvider(_persistentData);

            LoadDataOrInit();

            StateMachine();
            AsyncProcessor();
            UnityMainThread();

            LocalDataProvider();
            ThemeVisitors();
            Wallet();
            PersistentData();
            SelectedTheme();
            MapLibrary();
            MainMenuMusicManager();
            GameSettings();
            Achievements();
        }

        private void GameSettings() => Container.BindInterfacesAndSelfTo<GameSettings>().AsSingle().NonLazy();

        private void MainMenuMusicManager() => Container.BindInterfacesTo<MainMenuMusicManager>().AsSingle().WithArguments(_mainMenuMusic);

        private void MapLibrary() => Container.BindInstance(_mapLibrary);

        private void Achievements()
        {
            FirstMapBought firstMapBought = new(_persistentData, "91dfcd2c-715e-41dc-9808-c106e42f6127", _wallet);

            AchievementStorage achievementStorage = new(firstMapBought);

            Container.Bind<FirstMapBought>().FromInstance(firstMapBought).AsSingle();
            Container.Bind<AchievementStorage>().FromInstance(achievementStorage).AsSingle().NonLazy();
        }

        private void PersistentData() => Container.BindInstance(_persistentData);

        private void LocalDataProvider() => Container.Bind<LocalDataProvider>().FromInstance(_dataProvider).AsSingle().NonLazy();

        private void LoadDataOrInit()
        {
            if (!_dataProvider.TryLoad(out PersistentData loadedData))
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
            Container.Bind<ThemeSelector>().AsSingle().WithArguments(_persistentData);
            Container.Bind<ThemeUnlocker>().AsSingle().WithArguments(_persistentData);
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