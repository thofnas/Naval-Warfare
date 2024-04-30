using System.Linq;
using Data;
using EventBus;
using Events;
using Rewards;

namespace AchievementSystem
{
    public abstract class Achievement<T> : IAchievement where T : IEvent
    {
        public abstract AchievementInfo Info { get; protected set; }
        public abstract IReward Reward { get; }
        public AchievementID ID { get; }
        public bool IsUnlocked { get; private set; }

        protected TextData TextData => _languageData.TextData;
        private readonly PersistentData _persistentData;
        
        private readonly EventBinding<T> _checkEvent;
        private readonly LanguageData _languageData;

        protected Achievement(PersistentData persistentData, AchievementID id, LanguageData languageData)
        {
            _persistentData = persistentData;
            _languageData = languageData;
            ID = id;

            if (persistentData.PlayerData.UnlockedAchievements.Contains(ID))
                IsUnlocked = true;
            else
            {
                _checkEvent = new EventBinding<T>(CheckUnlockConditions);
                EventBus<T>.Register(_checkEvent);
            }
            
            EventBus<OnLanguageLoaded>.Register(new EventBinding<OnLanguageLoaded>(UpdateAchievementInfo));
        }

        protected abstract void UpdateAchievementInfo();

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