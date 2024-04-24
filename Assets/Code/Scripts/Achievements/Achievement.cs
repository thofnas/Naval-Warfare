using System;
using System.Linq;
using Data;
using EventBus;

namespace Achievements
{
    public abstract class Achievement<T> where T : IEvent
    {
        public abstract string Condition { get; protected set; }
        public abstract string Description { get; protected set; }
        private readonly PersistentData _persistentData;
        public readonly Guid Guid;
        public bool IsUnlocked { get; private set; }
        
        private readonly EventBinding<T> _eventBinding;

        protected Achievement(PersistentData persistentData, string guid)
        {
            Guid = new Guid(guid);
            
            if (!persistentData.PlayerData.UnlockedAchievements.Contains(Guid))
            {
                _eventBinding = new EventBinding<T>(CheckUnlockConditions);
                EventBus<T>.Register(_eventBinding);
            }
            else
                IsUnlocked = true;

            _persistentData = persistentData;
        }

        protected abstract void CheckUnlockConditions(T e);

        protected abstract void OnUnlock();

        protected void Unlock()
        {
            if (IsUnlocked) return;
            
            _persistentData.PlayerData.UnlockAchievement(Guid);
            
            IsUnlocked = true;
            
            EventBus<T>.Deregister(_eventBinding);
            
            OnUnlock();
        }
    }
}