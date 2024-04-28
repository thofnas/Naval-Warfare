using AI;
using Data;
using EventBus;
using Events;

namespace States.GameplayStates
{
    public class BattleResults : BaseState
    {
        private readonly GameplayManager _gameplayManager;
        private readonly Wallet _wallet;
        private readonly IDifficulty _difficulty;

        public BattleResults(GameplayManager gameplayManager, StateMachine.StateMachine stateMachine, Wallet wallet, IDifficulty difficulty) : base(stateMachine)
        {
            _gameplayManager = gameplayManager;
            _wallet = wallet;
            _difficulty = difficulty;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            EventBus<OnBattleEnded>.Invoke(new OnBattleEnded(_gameplayManager.LoserCharacterType == CharacterType.Enemy));

            if (_gameplayManager.LoserCharacterType != CharacterType.Enemy) return;
            
            _wallet.AddMoney(_difficulty.GetWinMoneyAmount());
        }
    }
}