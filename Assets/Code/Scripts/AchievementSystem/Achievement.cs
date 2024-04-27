using System.Linq;
using Data;
using EventBus;
using Rewards;

namespace AchievementSystem
{
    public abstract class Achievement<T> : IAchievement where T : IEvent
    {
        public abstract AchievementInfo Info { get; }
        public abstract IReward Reward { get; }
        public AchievementID ID { get; }
        public bool IsUnlocked { get; private set; }

        private readonly PersistentData _persistentData;
        
        private readonly EventBinding<T> _checkEvent;

        protected Achievement(PersistentData persistentData, AchievementID id)
        {
            _persistentData = persistentData;
            ID = id;

            if (persistentData.PlayerData.UnlockedAchievements.Contains(ID))
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
            
            _persistentData.PlayerData.UnlockAchievement(ID);
            
            IsUnlocked = true;
            
            EventBus<T>.Deregister(_checkEvent);
            
            Reward.Award();
            
            OnUnlock();
        }

        private void CheckUnlockConditions(T eventArgs)
        {
            if (AreConditionsMet(eventArgs))
                Unlock();
        }
    }
}