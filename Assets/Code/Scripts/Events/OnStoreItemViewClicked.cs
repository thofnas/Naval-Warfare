using EventBus;
using UI.Elements;

namespace Events
{
    public struct OnStoreItemViewClicked : IEvent
    {
        public readonly StoreItemView StoreItemView;

        public OnStoreItemViewClicked(StoreItemView storeItemView)
        {
            StoreItemView = storeItemView;
        }
    }
}