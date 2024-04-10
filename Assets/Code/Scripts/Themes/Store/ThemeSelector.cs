using Data;
using Themes.Store;
using UnityEngine;
using Zenject;

namespace Themes
{
    public class ThemeSelector : IStoreItemVisitor
    {
        private readonly PersistentData _persistentData;

        [Inject]
        public ThemeSelector(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        public void Visit(StoreItem storeItem) => Visit((dynamic)storeItem);

        public void Visit(IslandsThemeItem islandsThemeItem) => _persistentData.PlayerData.SelectedIslandsThemeType = islandsThemeItem.IslandsType;

        public void Visit(OceanThemeItem oceanThemeItem) => _persistentData.PlayerData.SelectedOceanThemeType = oceanThemeItem.OceanType;
    }
}