using Data;
using Themes.Store;
using UnityEngine;
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

        public void Visit(StoreItem storeItem) => Visit((dynamic)storeItem);

        public void Visit(IslandsThemeItem islandsThemeItem)
        {
            _persistentData.PlayerData.OpenIslandsTheme(islandsThemeItem.IslandsType);
            Debug.Log("opened");
        }

        public void Visit(OceanThemeItem oceanThemeItem) => _persistentData.PlayerData.OpenOceanTheme(oceanThemeItem.OceanType);
    }
}