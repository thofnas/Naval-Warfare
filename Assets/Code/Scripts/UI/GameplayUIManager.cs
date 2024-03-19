using System;
using EventBus;
using Events;
using StateMachine;
using States.GameplayUIStates;
using Themes;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

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
        private GameManager _gameManager;
        private StateMachine.StateMachine _stateMachine;
        
        private EventBinding<OnBattleStateEntered> _onBattleStateEntered;
        private EventBinding<OnBattleResultStateEntered> _onBattleResultStateEntered;

        [Inject]
        private void Construct(GameManager gameManager, Level level, StateMachine.StateMachine stateMachine,
            Theme theme)
        {
            _gameManager = gameManager;
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
            _onBattleStateEntered = new EventBinding<OnBattleStateEntered>(_ => _stateMachine.SwitchState(Battle));
            EventBus<OnBattleStateEntered>.Register(_onBattleStateEntered);
            _onBattleResultStateEntered =
                new EventBinding<OnBattleResultStateEntered>(_ => _stateMachine.SwitchState(BattleResults));
            EventBus<OnBattleResultStateEntered>.Register(_onBattleResultStateEntered);
        }

        private void OnDestroy()
        {
            EventBus<OnBattleStateEntered>.Deregister(_onBattleStateEntered);
            EventBus<OnBattleResultStateEntered>.Deregister(_onBattleResultStateEntered);
        }
    }
}