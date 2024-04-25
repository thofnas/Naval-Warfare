using System;
using System.Linq;
using Data;
using EventBus;

namespace Achievements
{
    public abstract class Achievement<T> : IAchievement where T : IEvent
    {
        public abstract string Name { get; }
        public abstract string UnlockCondition { get; }
        public Guid Guid { get; }
        public bool IsUnlocked { get; private set; }
        
        private readonly PersistentData _persistentData;
        
        private readonly EventBinding<T> _checkEvent;

        protected Achievement(PersistentData persistentData, string guid)
        {
            _persistentData = persistentData;
            Guid = new Guid(guid);

            if (persistentData.PlayerData.UnlockedAchievements.Contains(Guid))
                IsUnlocked = true;
            else
            {
                _checkEvent = new EventBinding<T>(CheckUnlockConditions);
                EventBus<T>.Register(_checkEvent);
            }
        }

        protected abstract bool AreConditionsMet(T eventArgs);

        protected abstract void OnUnlock();

        private void Unlock()
        {
            if (IsUnlocked) return;
            
            _persistentData.PlayerData.UnlockAchievement(Guid);
            
            IsUnlocked = true;
            
            EventBus<T>.Deregister(_checkEvent);
            
            OnUnlock();
        }

        private void CheckUnlockConditions(T eventArgs)
        {
            if (AreConditionsMet(eventArgs))
                Unlock();
        }
    }
}