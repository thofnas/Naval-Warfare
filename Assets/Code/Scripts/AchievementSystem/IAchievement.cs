using Rewards;

namespace AchievementSystem
{
    public interface IAchievement
    {
        public AchievementInfo Info { get; }
        public IReward Reward { get; }
        public AchievementID ID { get; }
        public bool IsUnlocked { get; }
    }
}