using System;
using EventBus;
using Events;
using StateMachine;
using States.GameplayUIStates;
using Themes;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using BaseState = States.GameplayStates.BaseState;

namespace UI
{
    [DisallowMultipleComponent]
    public class GameplayUIManager : MonoBehaviour
    {
        public Theme Theme { get; private set; }
        public PlacingShips PlacingShips { get; private set; }
        public Battle Battle { get; private set; }
        public BattleResults BattleResults { get; private set; }
        
        [SerializeField] private StyleSheet _placingShipsStyleSheet;
        [SerializeField] private StyleSheet _battleStyleSheet;
        [SerializeField] private StyleSheet _battleResultsStyleSheet;
        private StateMachine.StateMachine _stateMachine;
        
        private EventBinding<OnGameplayStateChanged> _onGameplayStateChanged;

        [Inject]
        private void Construct(LevelManager levelManager, StateMachine.StateMachine stateMachine,
            Theme theme)
        {
            _stateMachine = stateMachine;
            Theme = theme;
        }

        private void Awake()
        {
            // defining states
            PlacingShips = new PlacingShips(this, _placingShipsStyleSheet);
            Battle = new Battle(this, _battleStyleSheet);
            BattleResults = new BattleResults(this, _battleResultsStyleSheet);

            // set first state
            _stateMachine.SetState(PlacingShips);
        }

        private void Start()
        {
            _onGameplayStateChanged =
                new EventBinding<OnGameplayStateChanged>(e =>
                {
                    if (e.NewState == typeof(States.GameplayStates.Battle))
                        _stateMachine.SwitchState(Battle);
                    
                    if (e.NewState == typeof(States.GameplayStates.BattleResults))
                        _stateMachine.SwitchState(BattleResults);
                });
            
            EventBus<OnGameplayStateChanged>.Register(_onGameplayStateChanged);
        }

        private void OnDestroy()
        {
            EventBus<OnGameplayStateChanged>.Deregister(_onGameplayStateChanged);
        }
    }
}