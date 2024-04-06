using System;
using EventBus;
using Events;

namespace States.GameplayStates
{
    public class Battle : BaseState
    {
        private readonly LevelManager _levelManager;
        private readonly Action _onStateExit;
        private EventBinding<OnCameraOrthographicSizeChanged> _onCameraOrthographicSizeChanged;

        public Battle(LevelManager levelManager, StateMachine.StateMachine stateMachine, Action onStateExit) : base(stateMachine)
        {
            _levelManager = levelManager;
            _onStateExit = onStateExit;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            EventBus<OnBattleStateEntered>.Invoke(new OnBattleStateEntered());

            _levelManager.MoveGridsToBattle();
            
            _onCameraOrthographicSizeChanged = new EventBinding<OnCameraOrthographicSizeChanged>(_levelManager.MoveGridsToBattle);
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