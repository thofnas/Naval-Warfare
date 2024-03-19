using System.Collections.Generic;
using Themes;
using Themes.Store;
using UI;
using UI.Elements;
using Zenject;

namespace Infrastructure
{
    public class MainMenuInstaller : MonoInstaller
    {        
        private Theme _playerTheme;

        [Inject]
        private void Construct(Theme playerTheme)
        {
            _playerTheme = playerTheme;
        }
        
        public override void InstallBindings()
        {
            CharactersThemes();
            BackgroundAnimator();
            StorePanelFactory();
        }

        private void StorePanelFactory()
        {
            Container.BindFactory<IEnumerable<StoreItem>, StorePanel, StorePanel.Factory>().AsTransient();
        }

        private void CharactersThemes() => Container.Bind<CharactersThemes>().AsSingle().WithArguments(_playerTheme, GameResources.Instance.AITheme).NonLazy();
        
        private void BackgroundAnimator() => Container.BindInterfacesAndSelfTo<BackgroundAnimator>().FromComponentInNewPrefab(GameResources.Instance.BackgroundPrefab).AsSingle().NonLazy();
    }
}