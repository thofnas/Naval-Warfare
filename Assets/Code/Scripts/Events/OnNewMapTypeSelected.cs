using EventBus;
using Map;

namespace Events
{
    public struct OnNewMapTypeSelected : IEvent
    {
        public readonly MapType MapType;

        public OnNewMapTypeSelected(MapType mapType)
        {
            MapType = mapType;
        }
    }
}