using System.Collections.Generic;
using Map;
using Themes.Store;
using UI;
using UI.Elements;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class MainMenuInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private MainMenuUIManager _mainMenuUIManager;

        public void Initialize() => Container.InstantiatePrefab(_mainMenuUIManager);

        public override void InstallBindings()
        {
            This();
            
            BackgroundAnimator();
            StorePanelFactory();
            MapSelector();
        }

        private void This() => 
            Container.BindInterfacesTo<MainMenuInstaller>().FromInstance(this).AsSingle();

        private void MapSelector() => 
            Container.Bind<MapSelector>().AsSingle().NonLazy();
        
        private void BackgroundAnimator() => 
            Container.BindInterfacesAndSelfTo<BackgroundAnimator>().FromComponentInNewPrefab(GameResources.Instance.BackgroundPrefab).AsSingle().NonLazy();

        private void StorePanelFactory() => 
            Container.BindFactory<IEnumerable<StoreItem>, MapType, StorePanel, StorePanel.Factory>().AsTransient();
    }
}