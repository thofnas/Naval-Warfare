using Data;
using Events;
using UnityEngine;

namespace Achievements
{
    public class FirstMapBought : Achievement<OnThemeUnlocked>
    {
        public override string Name => "First purchase";
        public override string UnlockCondition => "Buy your first map";

        public FirstMapBought(PersistentData persistentData, string guid) : base(persistentData, guid)
        {
        }
 
        protected override bool AreConditionsMet(OnThemeUnlocked eventArgs)
        {
            return eventArgs.IsPurchasable;
        }

        protected override void OnUnlock()
        {
            Debug.Log($"Unlocked {nameof(FirstMapBought)} achievement");
        }
    }
}