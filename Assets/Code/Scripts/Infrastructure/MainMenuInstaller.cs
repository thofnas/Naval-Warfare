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
        private ThemeSettings _playerThemeSettings;

        [Inject]
        private void Construct(ThemeSettings playerThemeSettings)
        {
            _playerThemeSettings = playerThemeSettings;
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

        private void CharactersThemes() => Container.Bind<CharactersThemes>().AsSingle().WithArguments(_playerThemeSettings, GameResources.Instance.AIThemeSettings).NonLazy();
        
        private void BackgroundAnimator() => Container.BindInterfacesAndSelfTo<BackgroundAnimator>().FromComponentInNewPrefab(GameResources.Instance.BackgroundPrefab).AsSingle().NonLazy();
    }
}