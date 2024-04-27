using System;
using Misc;

namespace AchievementSystem
{
    public struct AchievementID
    {
        private Guid Guid { get; }

        private AchievementID(Guid guid)
        {
            Guid = guid;
        }

        private AchievementID(NonEmptyString value)
        {
            if (!Guid.TryParse(((string)value).Trim(), out Guid guid))
                throw new ArgumentException("Cannot parse into guid", nameof(value));
            
            Guid = guid;
        }

        public static implicit operator AchievementID(Guid value) => new(value);
        public static implicit operator AchievementID(NonEmptyString value) => new(value);
        public static implicit operator AchievementID(string value) => new(value);
        public static implicit operator Guid(AchievementID value) => value.Guid;
    }
}