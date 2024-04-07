using System.Linq;
using Data;
using Zenject;

namespace Themes.Store
{
    public class OwnedThemesChecker : IStoreItemVisitor
    {
        public bool IsOwned { get; private set; }
        
        private readonly PersistentData _persistentData;

        [Inject]
        public OwnedThemesChecker(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        public void Visit(StoreItem storeItem) => Visit((dynamic)storeItem);

        public void Visit(IslandsThemeItem islandsThemeItem) => IsOwned = _persistentData.PlayerData.OwnedIslandsThemesList.Contains(islandsThemeItem.IslandsType);

        public void Visit(OceanThemeItem oceanThemeItem) => IsOwned = _persistentData.PlayerData.OwnedOceanThemesList.Contains(oceanThemeItem.OceanType);
    }
}