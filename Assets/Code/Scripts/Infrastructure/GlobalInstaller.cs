﻿using System;
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
        [SerializeField] private StoreContent _storeContent;
        [SerializeField] private EventReference _mainMenuMusic;
        [SerializeField] private UISoundLibrary _uiSoundLibrary;
        private PersistentData _persistentData;
        private LocalDataProvider _localDataProvider;
        private Wallet _wallet;
        private LanguageProvider _languageProvider;
        private LanguageData _languageData;
        private ThemeUnlocker _themeUnlocker;


        public override void InstallBindings()
        {
            BetterStreamingAssets.Initialize();

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
            UISoundsManager();
            VolumeAdjuster();
            GameSettings();
            
            Achievements();
        }

        private void UISoundsManager() => Container.BindInterfacesTo<UISoundsManager>().AsSingle().WithArguments(_uiSoundLibrary);

        private void VolumeAdjuster() => Container.Bind<VolumeAdjuster>().AsSingle().NonLazy();

        private void LanguageData() => Container.BindInstance(_languageData);

        private void GameSettings() => Container.BindInterfacesAndSelfTo<GameSettings>().AsSingle().NonLazy();

        private void MainMenuMusicManager() => Container.BindInterfacesTo<MainMenuMusicManager>().AsSingle().WithArguments(_mainMenuMusic);

        private void MapLibrary() => Container.BindInstance(_mapLibrary);

        private void Achievements()
        {
            FirstMapBought firstMapBought = new(_persistentData, "91dfcd2c-715e-41dc-9808-c106e42f6127", _wallet, _languageData);
            WinFiveTimes winFiveTimes = new(_persistentData, "12255d3a-e0e7-42f3-9be6-47598a873248", _languageData, _storeContent, _themeUnlocker);
            WinTenTimes winTenTimes = new(_persistentData, "c74397d2-9fa4-452f-aadc-13ad8f47576b", _languageData, _storeContent, _themeUnlocker);
            
            AchievementStorage achievementStorage = new(firstMapBought, winFiveTimes, winTenTimes);

            Container.Bind<FirstMapBought>().FromInstance(firstMapBought).AsSingle();
            Container.Bind<WinFiveTimes>().FromInstance(winFiveTimes).AsSingle();
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