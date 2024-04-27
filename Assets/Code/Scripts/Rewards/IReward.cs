using Misc;

namespace Rewards
{
    public interface IReward
    {
        public int Amount { get; }
        public NonEmptyString Name { get; }
        public void Award();
    }
}