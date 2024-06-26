﻿using Data;
using EventBus;
using Events;
using Zenject;

namespace Themes.Store
{
    public class ThemeUnlocker : IStoreItemVisitor
    {
        private readonly PersistentData _persistentData;

        [Inject]
        public ThemeUnlocker(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        public void Visit(StoreItem storeItem)
        {
            Visit((dynamic)storeItem);
            
            EventBus<OnThemeUnlocked>.Invoke(new OnThemeUnlocked(isPurchasable: storeItem.IsPurchasable));
        }

        public void Visit(IslandsThemeItem islandsThemeItem) => _persistentData.PlayerData.OpenIslandsTheme(islandsThemeItem.IslandsType);

        public void Visit(OceanThemeItem oceanThemeItem) => _persistentData.PlayerData.OpenOceanTheme(oceanThemeItem.OceanType);
    }
}