using EventBus;

namespace Events
{
    public struct OnStoreItemUnlocked : IEvent
    {
        public readonly bool IsPurchasable;

        public OnStoreItemUnlocked(bool isPurchasable)
        {
            IsPurchasable = isPurchasable;
        }
    }
}