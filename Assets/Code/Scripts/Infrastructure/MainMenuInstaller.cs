using System.Collections.Generic;
using Audio;
using FMODUnity;
using Map;
using Themes;
using Themes.Store;
using UI.Elements;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private EventReference _backgroundMusic;
        
        public override void InstallBindings()
        {
            BackgroundAnimator();
            StorePanelFactory();
            MapSelector();

            Container.BindInterfacesAndSelfTo<MainMenuMusicManager>().AsSingle().WithArguments(_backgroundMusic);
        }

        private void MapSelector() => 
            Container.Bind<MapSelector>().AsSingle().NonLazy();
        
        private void BackgroundAnimator() => 
            Container.BindInterfacesAndSelfTo<BackgroundAnimator>().FromComponentInNewPrefab(GameResources.Instance.BackgroundPrefab).AsSingle().NonLazy();

        private void StorePanelFactory() => 
            Container.BindFactory<IEnumerable<StoreItem>, MapType, StorePanel, StorePanel.Factory>().AsTransient();
    }
}