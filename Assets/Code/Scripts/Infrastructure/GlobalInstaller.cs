using System;
using Audio;
using Data;
using FMODUnity;
using Map;
using Misc;
using Themes;
using Themes.Store;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        

        public override void InstallBindings()
        {
            _persistentData = new PersistentData();
            _dataProvider = new LocalDataProvider(_persistentData);

            LoadDataOrInit();

            BindStateMachine();
            BindAsyncProcessor();
            BindUnityMainThread();

            BindLocalDataProvider();
            BindThemeVisitors();
            BindWallet();

            BindPersistentData();
            
            SelectedTheme();

            Container.BindInstance(_mapLibrary);
            
            Container.BindInterfacesTo<MainMenuMusicManager>().AsSingle().WithArguments(_mainMenuMusic);
        }

        private void BindPersistentData() => Container.BindInstance(_persistentData);

        private void BindLocalDataProvider() => Container.Bind<LocalDataProvider>().FromInstance(_dataProvider).AsSingle().NonLazy();

        private void LoadDataOrInit()
        {
            if (!_dataProvider.TryLoad(out PersistentData loadedData))
            {
                _persistentData.PlayerData = new PlayerData();
                return;
            }

            _persistentData = loadedData;
        }

        private void BindWallet() => Container.Bind<Wallet>().AsSingle().WithArguments(_persistentData);

        private void BindThemeVisitors()
        {
            Container.Bind<ThemeSelector>().AsSingle().WithArguments(_persistentData);
            Container.Bind<ThemeUnlocker>().AsSingle().WithArguments(_persistentData);
            Container.Bind<SelectedThemeChecker>().AsTransient().WithArguments(_persistentData);
            Container.Bind<OwnedThemesChecker>().AsTransient().WithArguments(_persistentData);
        }

        private void BindUnityMainThread() => Container.Bind<UnityMainThread>().FromNewComponentOnNewGameObject().AsSingle();

        private void BindStateMachine() => Container.BindInterfacesAndSelfTo<StateMachine.StateMachine>().AsTransient();
        
        private void BindAsyncProcessor() => Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();

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