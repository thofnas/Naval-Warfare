using Data;
using EventBus;
using Events;
using Zenject;

namespace Map
{
    public class MapSelector
    {
        private readonly PersistentData _persistentData;

        [Inject]
        public MapSelector(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        public void Select(MapType mapType)
        {
            _persistentData.PlayerData.SelectedMapType = mapType;
            EventBus<OnNewMapTypeSelected>.Invoke(new OnNewMapTypeSelected(mapType));
        }
    }
}