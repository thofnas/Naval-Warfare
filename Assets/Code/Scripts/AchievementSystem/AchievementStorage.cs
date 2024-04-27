using System.Collections.Generic;

namespace AchievementSystem
{
    public class AchievementStorage
    {
        private readonly List<IAchievement> _achievements = new();
        
        public IEnumerable<IAchievement> Achievements => _achievements;
            
        public AchievementStorage(params IAchievement[] achievements)
        {
            foreach (IAchievement achievement in achievements)
            {
                _achievements.Add(achievement);
            }
        }
    }
}