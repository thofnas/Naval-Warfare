using System;
using Events;
using Scripts.EventBus;

namespace States.GameplayStates
{
    public class Battle : BaseState
    {
        private readonly Level _level;
        private readonly Action _onStateExit;
        private EventBinding<OnCameraOrthographicSizeChanged> _onCameraOrthographicSizeChanged;

        public Battle(TurnSystem turnSystem, Level level, Action onStateExit) : base(turnSystem)
        {
            _level = level;
            _onStateExit = onStateExit;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            EventBus<OnBattleStateEntered>.Invoke(new OnBattleStateEntered());

            _level.MoveGridsToBattle();
            TurnSystem.NextTurn();
            
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