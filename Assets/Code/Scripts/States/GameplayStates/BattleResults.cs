using EventBus;
using Events;

namespace States.GameplayStates
{
    public class BattleResults : BaseState
    {
        public BattleResults(StateMachine.StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            EventBus<OnBattleResultStateEntered>.Invoke(new OnBattleResultStateEntered());
        }
    }
}