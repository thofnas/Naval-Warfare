using System.Collections.Generic;
using Map;
using Themes.Store;
using UI;
using UI.Elements;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuUIManager _mainMenuUIManager;

        public override void InstallBindings()
        {
            BackgroundAnimator();
            StorePanelFactory();
            MapSelector();
            Container.InstantiatePrefab(_mainMenuUIManager);
        }

        private void MapSelector() => 
            Container.Bind<MapSelector>().AsSingle().NonLazy();
        
        private void BackgroundAnimator() => 
            Container.BindInterfacesAndSelfTo<BackgroundAnimator>().FromComponentInNewPrefab(GameResources.Instance.BackgroundPrefab).AsSingle().NonLazy();

        private void StorePanelFactory() => 
            Container.BindFactory<IEnumerable<StoreItem>, MapType, StorePanel, StorePanel.Factory>().AsTransient();
    }
}