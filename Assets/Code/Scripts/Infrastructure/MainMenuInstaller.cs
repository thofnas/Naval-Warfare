﻿using System.Collections.Generic;
using Map;
using Themes;
using Themes.Store;
using UI.Elements;
using Zenject;

namespace Infrastructure
{
    public class MainMenuInstaller : MonoInstaller
    {        
        public override void InstallBindings()
        {
            BackgroundAnimator();
            StorePanelFactory();
            MapSelector();
        }

        private void MapSelector() => 
            Container.Bind<MapSelector>().AsSingle().NonLazy();
        
        private void BackgroundAnimator() => 
            Container.BindInterfacesAndSelfTo<BackgroundAnimator>().FromComponentInNewPrefab(GameResources.Instance.BackgroundPrefab).AsSingle().NonLazy();

        private void StorePanelFactory() => 
            Container.BindFactory<IEnumerable<StoreItem>, MapType, StorePanel, StorePanel.Factory>().AsTransient();
    }
}