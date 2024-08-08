using AI;
using Data;
using EventBus;
using Events;
using Infrastructure;
using Themes;
using UI.GameplayUIStates;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class GameplayUIManager : MonoBehaviour
    {
        public SelectedTheme SelectedTheme { get; private set; }
        public PlacingShips PlacingShips { get; private set; }
        public Battle Battle { get; private set; }
        public BattleResults BattleResults { get; private set; }
        
        public LanguageData LanguageData { get; private set; }
        
        [SerializeField] private StyleSheet _placingShipsStyleSheet;
        [SerializeField] private StyleSheet _battleStyleSheet;
        [SerializeField] private StyleSheet _battleResultsStyleSheet;
        private StateMachine.StateMachine _stateMachine;
        private Wallet _wallet;
        private IDifficulty _difficulty;
        
        private EventBinding<OnGameplayStateChanged> _onGameplayStateChanged;

        [Inject]
        private void Construct(LevelManager levelManager, StateMachine.StateMachine stateMachine, LanguageData languageData,
            SelectedTheme selectedTheme, Wallet wallet, IDifficulty difficulty)
        {
            _stateMachine = stateMachine;
            LanguageData = languageData; 
            SelectedTheme = selectedTheme;
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
                    if (e.NewState == typeof(GameplayStates.Battle))
                        _stateMachine.SwitchState(Battle);
                    
                    if (e.NewState == typeof(GameplayStates.BattleResults))
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