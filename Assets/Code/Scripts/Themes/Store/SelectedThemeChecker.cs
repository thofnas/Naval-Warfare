using Data;
using Zenject;

namespace Themes.Store
{
    public class SelectedThemeChecker : IStoreItemVisitor
    {
        public bool IsSelected { get; private set; }
        
        private readonly PersistentData _persistentData;

        [Inject]
        public SelectedThemeChecker(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        public void Visit(StoreItem storeItem) => Visit((dynamic)storeItem);

        public void Visit(IslandsThemeItem islandsThemeItem) => IsSelected = _persistentData.PlayerData.SelectedIslandsTheme == islandsThemeItem.IslandsType;

        public void Visit(OceanThemeItem oceanThemeItem) => IsSelected = _persistentData.PlayerData.SelectedOceanTheme == oceanThemeItem.OceanType;
    }
}