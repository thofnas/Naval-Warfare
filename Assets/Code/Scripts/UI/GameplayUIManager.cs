using Enemy;
using EventBus;
using Events;
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
        private StateMachine.StateMachine _stateMachine;
        private Wallet _wallet;
        private IDifficulty _difficulty;
        
        private EventBinding<OnGameplayStateChanged> _onGameplayStateChanged;

        [Inject]
        private void Construct(LevelManager levelManager, StateMachine.StateMachine stateMachine,
            Theme theme, Wallet wallet, IDifficulty difficulty)
        {
            _stateMachine = stateMachine;
            Theme = theme;
            _wallet = wallet;
            _difficulty = difficulty;
        }

        private void Awake()
        {
            // defining states
            PlacingShips = new PlacingShips(this, _placingShipsStyleSheet);
            Battle = new Battle(this, _battleStyleSheet);
            BattleResults = new BattleResults(this, _battleResultsStyleSheet, _wallet, _difficulty);

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