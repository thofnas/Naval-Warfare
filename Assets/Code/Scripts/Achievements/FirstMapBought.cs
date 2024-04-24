using System;
using Data;
using Events;
using UnityEngine;

namespace Achievements
{
    public class FirstMapBought : Achievement<OnThemeUnlocked>
    {
        public override string Condition { get; protected set; } = "Buy your first map";
        public override string Description { get; protected set; } = "";

        public FirstMapBought(PersistentData persistentData, string guid) : base(persistentData, guid)
        {
        }

        protected override void CheckUnlockConditions(OnThemeUnlocked e)
        {
            if (!e.IsPurchasable) return;
            
            Unlock();
        }

        protected override void OnUnlock()
        {
            Debug.Log($"Unlocked {nameof(FirstMapBought)} achievement");
        }
    }
}