using Data;
using Misc;
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
            Container.Bind<PersistentData>().AsSingle();
        }

        private void UnityMainThread() => Container.Bind<UnityMainThread>().FromNewComponentOnNewGameObject().AsSingle();

        private void StateMachine() => Container.BindInterfacesAndSelfTo<StateMachine.StateMachine>().AsTransient();
        
        private void AsyncProcessor() => Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();
    }
}