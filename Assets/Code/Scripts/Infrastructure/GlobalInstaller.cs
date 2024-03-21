using Data;
using Misc;
using Themes;
using Zenject;

namespace Infrastructure
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            StateMachine();
            AsyncProcessor();
            UnityMainThread();
            PersistentData();
            ThemeVisitors();
            Wallet();
        }

        private void Wallet() => Container.Bind<Wallet>().AsSingle();

        private void PersistentData() => Container.Bind<PersistentData>().AsSingle();

        private void ThemeVisitors()
        {
            Container.Bind<ThemeSelector>().AsSingle();
            Container.Bind<ThemeUnlocker>().AsSingle();
            Container.Bind<SelectedThemeChecker>().AsTransient();
            Container.Bind<OwnedThemesChecker>().AsTransient();
        }

        private void UnityMainThread() => Container.Bind<UnityMainThread>().FromNewComponentOnNewGameObject().AsSingle();

        private void StateMachine() => Container.BindInterfacesAndSelfTo<StateMachine.StateMachine>().AsTransient();
        
        private void AsyncProcessor() => Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();
    }
}