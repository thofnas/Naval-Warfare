using UnityEngine;

namespace Achievements
{
    public abstract class Achievement : ScriptableObject
    {
        [field: SerializeField] public AchievementType AchievementType { get; protected set; }
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public bool IsUnlocked { get; protected set; }
        
        public abstract void CheckUnlockingConditions();

        public virtual void OnUnlock()
        {
            if (IsUnlocked) return;
        }
    }
}