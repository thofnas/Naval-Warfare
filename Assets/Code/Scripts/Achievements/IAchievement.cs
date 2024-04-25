using System;

namespace Achievements
{
    public interface IAchievement
    {
        public string Name { get; }
        public string UnlockCondition { get; }
        public Guid Guid { get; }
        public bool IsUnlocked { get; }
    }
}