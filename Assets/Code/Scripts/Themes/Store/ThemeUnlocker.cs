using Data;
using Events;
using Themes.Store;
using EventBus;
using Zenject;

namespace Themes
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
            
            EventBus<OnStoreItemUnlocked>.Invoke(new OnStoreItemUnlocked(isPurchasable: storeItem.IsPurchasable));
        }

        public void Visit(IslandsThemeItem islandsThemeItem) => _persistentData.PlayerData.OpenIslandsTheme(islandsThemeItem.IslandsType);

        public void Visit(OceanThemeItem oceanThemeItem) => _persistentData.PlayerData.OpenOceanTheme(oceanThemeItem.OceanType);
    }
}