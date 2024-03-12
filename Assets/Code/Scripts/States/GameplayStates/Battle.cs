using System;
using EventBus;
using Events;

namespace States.GameplayStates
{
    public class Battle : BaseState
    {
        private readonly TurnSystem _turnSystem;
        private readonly Level _level;
        private readonly Action _onStateExit;
        private EventBinding<OnCameraOrthographicSizeChanged> _onCameraOrthographicSizeChanged;

        public Battle(TurnSystem turnSystem, Level level, StateMachine.StateMachine stateMachine, Action onStateExit) : base(stateMachine)
        {
            _turnSystem = turnSystem;
            _level = level;
            _onStateExit = onStateExit;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            EventBus<OnBattleStateEntered>.Invoke(new OnBattleStateEntered());

            _level.MoveGridsToBattle();
            _turnSystem.NextTurn();
            
            _onCameraOrthographicSizeChanged = new EventBinding<OnCameraOrthographicSizeChanged>(_level.MoveGridsToBattle);
            EventBus<OnCameraOrthographicSizeChanged>.Register(_onCameraOrthographicSizeChanged);
        }

        public override void OnExit()
        {
            base.OnExit();
            _onStateExit();
            EventBus<OnCameraOrthographicSizeChanged>.Deregister(_onCameraOrthographicSizeChanged);
        }
    }
}