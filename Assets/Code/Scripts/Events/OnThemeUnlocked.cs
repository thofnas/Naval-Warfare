using EventBus;

namespace Events
{
    public struct OnThemeUnlocked : IEvent
    {
        public readonly bool IsPurchasable;

        public OnThemeUnlocked(bool isPurchasable)
        {
            IsPurchasable = isPurchasable;
        }
    }
}