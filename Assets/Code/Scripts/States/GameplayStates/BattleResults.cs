using Data;
using Enemy;

namespace States.GameplayStates
{
    public class BattleResults : BaseState
    {
        private readonly GameplayManager _gameplayManager;
        private readonly Wallet _wallet;
        private readonly IDifficulty _difficulty;
        private readonly LocalDataProvider _localDataProvider;

        public BattleResults(GameplayManager gameplayManager, StateMachine.StateMachine stateMachine, Wallet wallet, IDifficulty difficulty, LocalDataProvider localDataProvider) : base(stateMachine)
        {
            _gameplayManager = gameplayManager;
            _wallet = wallet;
            _difficulty = difficulty;
            _localDataProvider = localDataProvider;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (_gameplayManager.LoserCharacterType != CharacterType.Enemy) return;
            
            
            _wallet.AddMoney(_difficulty.GetWinMoneyAmount());
            _localDataProvider.Save();
        }
    }
}