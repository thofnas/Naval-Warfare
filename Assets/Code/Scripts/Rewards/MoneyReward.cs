using Misc;

namespace Rewards
{
    public class MoneyReward : IReward
    {
        public int Amount { get; }
        public NonEmptyString Name { get; }

        private readonly Wallet _wallet;

        public MoneyReward(int amount, Wallet wallet)
        {
            Amount = amount;
            Name = "Coins";
            
            _wallet = wallet;
        }

        public void Award() => _wallet.AddMoney(Amount);
    }
}