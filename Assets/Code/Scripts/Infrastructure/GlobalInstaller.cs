using Data;
using Misc;
using Themes;
using Zenject;

namespace Infrastructure
{
    public class GlobalInstaller : MonoInstaller
    {
        private PersistentData _persistentData;
        private LocalDataProvider _dataProvider;

        public override void InstallBindings()
        {
            _persistentData = new PersistentData();
            _dataProvider = new LocalDataProvider(_persistentData);

            if (_dataProvider.TryLoad() == false)
                _persistentData.PlayerData = new PlayerData();
            
            StateMachine();
            AsyncProcessor();
            UnityMainThread();

            Container.Bind<PersistentData>().FromInstance(_persistentData).AsSingle();
            Container.Bind<LocalDataProvider>().FromInstance(_dataProvider).AsSingle().WithArguments(_persistentData);
            ThemeVisitors();
            Wallet();
        }

        private void Wallet() => Container.Bind<Wallet>().AsSingle().WithArguments(_persistentData);

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
    }
}