using Misc;

namespace AchievementSystem
{
    public record AchievementInfo(NonEmptyString Name, NonEmptyString UnlockCondition);
}